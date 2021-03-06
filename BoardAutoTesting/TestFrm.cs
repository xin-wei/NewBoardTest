using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.ServiceModel;
using System.Threading;
using System.Windows.Forms;
using BoardAutoTesting.BLL;
using BoardAutoTesting.DataExchange;
using BoardAutoTesting.Log;
using BoardAutoTesting.Model;
using BoardAutoTesting.Properties;
using BoardAutoTesting.RepairInfo;
using BoardAutoTesting.Service;
using BoardAutoTesting.Status;
using BoardAutoTesting.WipTracking;
using Commons;
using CraftControl;
using DevComponents.DotNetBar;
using DataReceivedEventArgs = Commons.DataReceivedEventArgs;

namespace BoardAutoTesting
{
    public partial class TestFrm : Office2007RibbonForm
    {
        private SerialPortUtil _serialPort = new SerialPortUtil();
        private string _strLastEsn;
        private string _strComRfid;
        private static bool _stopFlag;
        private bool _stopBeep;
        private bool _bCanIn = true;
        //SFIS
        private tCheckDataTestAteSoapClient _ate = new tCheckDataTestAteSoapClient();
        private tWipTrackingSoapClient _wip = new tWipTrackingSoapClient();
        private tRepairInfoSoapClient _repair = new tRepairInfoSoapClient();

        private SystemInfo _model;
        private readonly CenterServer _server = new CenterServer();
        private readonly CodeSoftHelper _csHelper = new CodeSoftHelper();

        private const int BeepInverval = 1000;

        public TestFrm()
        {
            InitializeComponent();
        }

        private void TestFrm_Load(object sender, EventArgs e)
        {
            InitShopFloor();
            CreateDefaultModel();
            InitMainForm();
            InitCodeSoft();
            InitLog();
            InitSerialPort();
            InitCountIni();
            InitRoutes();

            txtBarcode.Enabled = false;
            btnStartServer.Enabled = true;
            btnStopServer.Enabled = false;
            btnReset.Enabled = false;

            ThreadPool.SetMaxThreads(25, 25);
            ThreadPool.SetMinThreads(12, 12);

            txtIp.Text = HardwareInfoHelper.GetIPAddress();
            txtPort.Text = Resources.Port;

            ClientConnection.Ate = _ate;
            

            Thread t = new Thread(ChangeState) {IsBackground = true};
            t.Start();
        }

        private static void InitCountIni()
        {
            INIFileUtil iniFile = new INIFileUtil(
                string.Format(@"{0}\result.ini", Application.StartupPath));
            string lastTime = iniFile.IniReadValue(Resources.Section, "Time");
            List<LineInfo> infos = LineBll.GetModels();
            if (infos == null) return;

            if (lastTime != "" && DateTime.Parse(lastTime).DayOfYear == DateTime.Now.DayOfYear) return;

            iniFile.IniWriteValue(Resources.Section, "Time",
                DateTime.Now.ToString(CultureInfo.InvariantCulture));
                
            foreach (var line in infos)
            {
                iniFile.IniWriteValue(Resources.Section, line.CraftId, "0/0");
            }
        }

        private static void InitRoutes()
        {
            List<LineInfo> lstLines = LineBll.GetModels();
            if (lstLines == null)
            {
                MessageUtil.ShowError(Resources.UnconfigedCraft);
                Application.Exit();
                return;
            }

            AllRoutes.LstRoutes = lstLines.Select(line => line.RouteName).ToList();
        }

        private void InitSerialPort()
        {
            SerialPortUtil.SetPortNameValues(cbxPortName);
            SerialPortUtil.SetBauRateValues(cbxBaudRate);
            try
            {
                if (cbxPortName.Items.Count > 0)
                {
                    string portName = RegistryHelper.GetValue(
                        @"Software\TS\PTester\Tester\Port", "PortName");
                    if (string.IsNullOrEmpty(portName))
                        cbxPortName.SelectedIndex = 0;
                    else
                        cbxPortName.SelectedItem = portName;
                }

                if (cbxBaudRate.Items.Count > 0)
                {
                    string baudRate = RegistryHelper.GetValue(
                        @"Software\TS\PTester\Tester\Port", "BauRate");
                    if (string.IsNullOrEmpty(baudRate))
                        cbxBaudRate.SelectedIndex = 0;
                    else
                        cbxBaudRate.SelectedItem = baudRate;
                }
            }
            catch (Exception e)
            {
                MessageUtil.ShowError("请重新选择串口！");
                Logger.Glog.Info(e.Message);
            }

        }

        private static void InitLog()
        {
            string filepathname = Application.StartupPath + @"\Log";
            if (!(Directory.Exists(filepathname)))
            {
                Directory.CreateDirectory(filepathname);
                Debug.Assert(Directory.Exists(filepathname));
            }

            filepathname += @"\BoardTest_";
            Logger.Log(filepathname);
            Logger.Glog.Success("-----------------------");
        }

        private void InitCodeSoft()
        {
            _csHelper.OpenCodeSoft();
            if (!_csHelper.CheckFileExist(
                string.Format("{0}\\LAB\\Print.LAB", Application.StartupPath)))
            {
                MessageBox.Show(string.Format("未找到打印模板：{0}\\LAB\\Print.LAB",
                    Application.StartupPath));

                Close();
                Application.Exit();
            }

            _csHelper.OpenLabelFile(string.Format("{0}\\LAB\\Print.LAB",
                Application.StartupPath), false);
            _csHelper.SetPrintNum(1);
            ClientConnection.CsHelper = _csHelper; //注入codesoft
        }

        private void btnStartServer_Click(object sender, EventArgs e)
        {
            if (!_model.IsLogin)
            {
                MessageUtil.ShowWarning(Resources.InLogin);
                return;
            }

            string address = txtIp.Text;
            string port = txtPort.Text;
            if (address == "" || port == "")
                MessageUtil.ShowWarning(Resources.IpPortError);

            IPAddress ipAddress;
            bool bValidIp = IPAddress.TryParse(address, out ipAddress);
            if (!bValidIp)
                MessageUtil.ShowWarning(Resources.InvalidIp);

            _server.WatchConnection(address, int.Parse(port));

            btnStartServer.Enabled = false;
            btnStopServer.Enabled = true;
            btnReset.Enabled = true;
            txtIp.Enabled = false;
            txtPort.Enabled = false;
        }

        private void comPort_DataReceived(DataReceivedEventArgs e)
        {
            string cmd = e.DataReceived;
            if (cmd.Contains(CmdInfo.CanIn))
            {
                string tempRfid = cmd.Replace("*", "").Replace("#", "").Replace(":IN?", "").Trim();
                if (tempRfid.Length != 8) return;
                if (_strComRfid == tempRfid) return;
                _strComRfid = tempRfid;

                //超时没扫到就报错
                int startTick = Environment.TickCount;
                int endTick = Environment.TickCount;
                while (string.IsNullOrEmpty(_strLastEsn))
                {
                    if (endTick - startTick <= 3000)
                    {
                        endTick = Environment.TickCount;
                        continue;
                    }

                    OperationControl.ShowStatus(lblStatus, 
                        OperationControl.TypeList.Error, "NO ESN");

                    _strComRfid = "";
                    return;
                }
                _serialPort.WriteData(CmdInfo.GoInGet);
                
                Thread t = new Thread(() =>
                {
                    OperationControl.ShowStatus(lblStatus,
                        OperationControl.TypeList.Running, "Running");

                    if (string.IsNullOrEmpty(_strLastEsn) || 
                        string.IsNullOrEmpty(_strComRfid))
                    {
                        Logger.Glog.Info("怎么可能没有条码或RFID！");
                        NextEnd(_strLastEsn);
                        return;
                    }

                    ProductInfo product = ProductBll.GetModelByRfid(_strComRfid);
                    if (product != null)
                    {
                        product.ESN = _strLastEsn;
                        product.IsPass = ProductStatus.UnKnown.ToString();
                        product.RouteName = "NA";
                        product.CraftId = "NA";
                        product.CurrentIp = "NA";
                        product.OldIp = "NA";
                        product.ActionName = ProductAction.OnLine.ToString();
                        product.ATEIp = "NA";
                        ProductBll.SureToUpdateModel(product);
                    }
                    else
                    {
                        ProductInfo newProduct = new ProductInfo
                        {
                            RFID = _strComRfid,
                            ESN = _strLastEsn,
                            IsPass = ProductStatus.UnKnown.ToString(),
                            RouteName = "NA",
                            CraftId = "NA",
                            CurrentIp = "NA",
                            OldIp = "NA",
                            ActionName = ProductAction.OnLine.ToString(),
                            ATEIp = "NA"
                        };
                        ProductBll.InsertModel(newProduct);
                    }

                    while (!_bCanIn)
                    {
                        Thread.Sleep(100);
                    }

                    _serialPort.WriteData(CmdInfo.GoNext);
                    _bCanIn = false;

                    NextEnd(_strLastEsn);

                }) { IsBackground = true };
                t.Start();
            }

            if (cmd.Contains(CmdInfo.DoorOpen))
            {
                _serialPort.WriteData(CmdInfo.OpenGet);
                _bCanIn = true;
                return;
            }

            if (cmd.Contains(CmdInfo.DoorClose))
            {
                _serialPort.WriteData(CmdInfo.CloseGet);
                return;
            }

            if (cmd.Contains(CmdInfo.ProductGet))
            {
                _stopBeep = true;
                return;
            }

            if (!cmd.Contains(CmdInfo.GoNextOk)) return;
            _serialPort.WriteData(CmdInfo.GoNextOk);
            _bCanIn = true;
        }


        private void NextEnd(string sn)
        {
            if (!string.IsNullOrEmpty(sn))
                OperationControl.ShowStatus(lblStatus, 
                    OperationControl.TypeList.Incoming, sn);
            else
                OperationControl.ShowStatus(lblStatus, 
                    OperationControl.TypeList.Error, "NO ESN");

            _strLastEsn = "";
            _strComRfid = "";
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (btnConnect.Text == Resources.Connect)
            {
                if (cbxPortName.Items.Count <= 0)
                {
                    MessageUtil.ShowError(Resources.NoPort);
                    return;
                }

                if (cbxPortName.SelectedItem == null)
                {
                    MessageUtil.ShowError(Resources.NoPort);
                    return;
                }

                if (cbxBaudRate.SelectedItem == null)
                {
                    MessageUtil.ShowError(Resources.NoBaudRate);
                    return;
                }

                string portName = cbxPortName.SelectedItem.ToString();
                if (!SerialPortUtil.Exists(portName)) return;

                if (!_serialPort.IsOpen)//判断是否打开
                {
                    _serialPort = new SerialPortUtil(portName,
                        (SerialPortBaudRates)int.Parse(cbxBaudRate.SelectedItem.ToString()),
                        Parity.None,
                        SerialPortDatabits.EightBits, StopBits.One)
                    {
                        EndByte = (byte)'#'
                    };
                    _serialPort.DataReceived += comPort_DataReceived;
                    _serialPort.OpenPort();

                    RegistryHelper.SaveValue(@"Software\TS\PTester\Tester\Port",
                            "PortName", cbxPortName.SelectedItem.ToString());
                    RegistryHelper.SaveValue(@"Software\TS\PTester\Tester\Port",
                        "BauRate", cbxBaudRate.SelectedItem.ToString());
                }
                cbxBaudRate.Enabled = false;
                cbxPortName.Enabled = false;
                btnConnect.Text = Resources.Disconnect;
                txtBarcode.Enabled = true;
                txtBarcode.Focus();
            }
            else
            {
                cbxBaudRate.Enabled = true;
                cbxPortName.Enabled = true;
                btnConnect.Text = Resources.Connect;
                _serialPort.DataReceived -= comPort_DataReceived;
                _serialPort.DiscardBuffer();
                _serialPort.ClosePort();
                txtBarcode.Enabled = false;
            }
        }

        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            /*比对是否一致*/
            string strEsn = txtBarcode.Text;
            txtBarcode.Text = string.Empty;

            if (_strLastEsn == strEsn) return;

            /*检测是否是在该工单中*/
            if (_model.IsLogin)
            {
                DataTable dtTracking = ReleaseData.arrByteToDataTable(
                    _wip.Get_WIP_TRACKING("ESN", strEsn, "WOID,ERRFLAG"));

                if (dtTracking.Rows.Count <= 0)
                {
                    Beep();
                    MessageUtil.ShowError("ESN IS ERROR : 未找到当前ESN的信息");
                    return;
                }
                if (dtTracking.Rows.Count > 1)
                {
                    Beep();
                    MessageUtil.ShowError("ESN IS ERROR : ESN的信息存在多条");
                    return;
                }
                if (dtTracking.Rows[0]["WOID"].ToString() != _model.WoId)
                {
                    Beep();
                    MessageUtil.ShowError("ESN IS ERROR : 不属于当前工单");
                    return;
                }
                if (dtTracking.Rows[0]["ERRFLAG"].ToString() == "1")
                {
                    Beep();
                    MessageUtil.ShowError("ESN IS ERROR : 当前ESN为维修板");
                    return;
                }

                //检测途程
                string strResult = ClientConnection.Ate.Check_Route_ATE(strEsn,
                    "NA", ClientConnection.SysModel.WoId);
                if (!AllRoutes.LstRoutes.Contains(strResult))
                {
                    Beep();
                    MessageUtil.ShowError("ESN IS ERROR : 不属于当前站位");
                    return;
                }

            }

            _strLastEsn = strEsn;
        }

        /// <summary>
        /// 蜂鸣，单片机反馈*PRO:GET#后停止发送FAIL指令
        /// 三次还收不到就不管了，直接弹窗提醒
        /// </summary>
        private void Beep()
        {
            _serialPort.WriteData(CmdInfo.ProductFail);
            for (int i = 0; i < 3; i++)
            {
                int startTick = Environment.TickCount;
                int endTick = Environment.TickCount;
                while (endTick - startTick < BeepInverval)
                {
                    if (_stopBeep)
                    {
                        _stopBeep = false;
                        return;
                    }

                    Thread.Sleep(10);
                    endTick = Environment.TickCount;
                }
            }
        }

        private void CloseServer()
        {
            if (_server != null)
                _server.DoCleaning();
        }

        private void btnStopServer_Click(object sender, EventArgs e)
        {
            btnStartServer.Enabled = true;
            btnStopServer.Enabled = false;
            btnReset.Enabled = false;

            txtIp.Enabled = true;
            txtPort.Enabled = true;

            CloseServer();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(() =>
            {
                List<LineInfo> lstLineInfos = LineBll.GetModels();
                foreach (var line in lstLineInfos.Where(
                    line => CenterServer.DictConnections.ContainsKey(line.McuIp)))
                {
                    CenterServer.DictConnections[line.McuIp].IsOpenDoor = true;
                    CenterServer.DictConnections[line.McuIp].Rfid = "";
                    line.CraftEsn = "";
                    line.LineEsn = "";

                    if (!LineBll.SureToUpdateModel(line, "Mcu_Ip"))
                    {
                        //应该是不可能出现的情况
                        Logger.Glog.Info("", "TestFrm.btnReset_Click.SureToUpdateModel",
                            Resources.UpdateError);
                        return;
                    }
                    Thread.Sleep(500);
                }
            }) {IsBackground = true};
            t.Start();
        }

        private void btnPrintTest_Click(object sender, EventArgs e)
        {
            _csHelper.ClearVariables();
            Dictionary<string, object> dicVariables = new Dictionary<string, object>
            {
                {"ESN", "TEST"},
                {"CRAFT", "NA"},
                {"EC", "Craft00003"}
            };
            _csHelper.Fill_Label_Variables(dicVariables);
            _csHelper.PrintLabel();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!_model.IsLogin)
            {
                LoginFrm login = new LoginFrm();
                if (login.ShowDialog() == DialogResult.Yes)
                {
                    InitSystemInfo();
                    InitMainForm();
                    ClientConnection.SysModel = _model;
                }
            }
            else
            {
                CreateDefaultModel();
                InitMainForm();
            }
        }

        private void InitShopFloor()
        {
            BasicHttpBinding binding = new BasicHttpBinding
            {
                MaxReceivedMessageSize = int.MaxValue,
                MaxBufferPoolSize = int.MaxValue,
                MaxBufferSize = int.MaxValue
            };

            //"http://170.1.1.231/SFIS_WEBSER_TEST/tCheckDataTestAte.asmx"
            _ate = new tCheckDataTestAteSoapClient(binding,
                new EndpointAddress(_ate.Endpoint.Address.Uri.ToString()));

            //"http://170.1.1.231/SFIS_WEBSER_TEST/tWipTracking.asmx"
            _wip = new tWipTrackingSoapClient(binding,
                new EndpointAddress(_wip.Endpoint.Address.Uri.ToString()));


            _repair = new tRepairInfoSoapClient(binding,
                new EndpointAddress(_repair.Endpoint.Address.Uri.ToString()));
        }

        private void InitMainForm()
        {
            lblWorkOrder.Text = _model.WoId;
            lblUser.Text = string.Format("操作员：{0}-{1}", _model.UserId,
                _model.UserName);
            lblCraft.Text = string.Format("{0}/{1}", _model.PartNumber,
                _model.PartName);
            lblLine.Text = _model.LineId;
            OperationControl.ShowStatus(lblStatus, 
                OperationControl.TypeList.Running,
                "WAIT");
            btnLogin.Text = _model.IsLogin ? "用户注销" : "用户登入";
        }

        /// <summary>
        /// 每次登录的时候创建默认的SystemInfo对象，因为每天的工单不同，必须重新登录
        /// </summary>
        private void CreateDefaultModel()
        {
            _model = new SystemInfo
            {
                IsLogin = false,
                LineId = "-",
                UserId = "-",
                UserName = "-",
                PartName = "-",
                PartNumber = "-",
                WoId = "-",
                UserPwd = ""
            };
        }

        private void InitSystemInfo()
        {
            string path = Application.StartupPath + @"\config.bin";
            if (!File.Exists(path))
            {
                CreateDefaultModel();
            }
            else
            {
                IFormatter formatter = new BinaryFormatter();
                using (Stream stream = new FileStream(path,
                    FileMode.Open, FileAccess.Read))
                {
                    _model = (SystemInfo)formatter.Deserialize(stream);
                    stream.Close();
                }
            }

        }

        private void TestFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_csHelper != null)
                _csHelper.QuitCodeSoft();

            _stopFlag = true;
            Logger.Glog.Info("服务器关闭");
            Logger.Glog.Stop();
            Application.Exit();
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            SettingFrm setting = new SettingFrm();
            setting.ShowDialog();
        }

        private void ChangeState()
        {
            while (!_stopFlag)
            {
                List<LineInfo> lines = LineBll.GetModels();
                foreach (var line in lines)
                {
                    try
                    {
                        if (line.IsOut)
                            continue;

                        int craftNum = Convert.ToInt32(line.CraftId.Remove(0, 5));

                        UserCraft craft = (UserCraft) groupPanel1.Controls["craft" + craftNum];
                        SplitContainer split = (SplitContainer) craft.Controls["splitContainer1"];

                        Application.DoEvents();

                        ButtonX btnLine = (ButtonX) split.Panel2.Controls["btnLine"];
                        btnLine.BackColor = line.LineEsn != ""
                            ? Color.DarkGoldenrod
                            : Color.ForestGreen;

                        ButtonX btnCraft = (ButtonX) split.Panel1.Controls["btnCraft"];
                        btnCraft.BackColor = line.CraftEsn != ""
                            ? Color.DarkGoldenrod
                            : Color.ForestGreen;
                        if (line.IsRepair)
                            btnCraft.BackColor = Color.Red;

                        Application.DoEvents();

                        INIFileUtil iniFile = new INIFileUtil(
                            string.Format(@"{0}\result.ini", Application.StartupPath));
                        string result = iniFile.IniReadValue(Resources.Section, line.CraftId);

                        if (_stopFlag) return;
                        var line1 = line;
                        Invoke((EventHandler) delegate
                        {
                            string showResult = line1.RouteName + "\r\n" +
                                                "Pass/Fail" + "\r\n" + result;
                            btnCraft.Text = showResult;
                            btnLine.Text = line1.CraftId;
                        });

                        Application.DoEvents();
                    }
                    catch (Exception e)
                    {
                        Logger.Glog.Info(e.Message);
                        //如果出现了意外就重启刷新线程
                        if (!_stopFlag)
                        {
                            Thread t = new Thread(ChangeState) {IsBackground = true};
                            t.Start();
                        }
                        else
                            return;
                    }

                }

                Thread.Sleep(3000);
            }
        }
    }
}