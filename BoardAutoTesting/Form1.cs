using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using BoardAutoTesting.BLL;
using BoardAutoTesting.DataExchange;
using BoardAutoTesting.Log;
using BoardAutoTesting.Model;
using BoardAutoTesting.Properties;
using BoardAutoTesting.Status;

namespace BoardAutoTesting
{
    public partial class MainFrm : Form
    {
        private readonly CenterServer _server = new CenterServer();
        private CodeSoftHelper _csHelper = new CodeSoftHelper();

        public MainFrm()
        {
            InitializeComponent();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            

            string address = txtIp.Text;
            string port = txtPort.Text;
            if (address == "" || port == "")
                MessageBox.Show(Resources.IpPortError, Resources.ConfigError, 
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            IPAddress ipAddress;
            bool bValidIp = IPAddress.TryParse(address, out ipAddress);
            if (!bValidIp)
                MessageBox.Show(Resources.IpPortError, Resources.InvalidIp,
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            
            _server.WatchConnection(address, int.Parse(port));
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            _csHelper.OpenCodeSoft();
            if (_csHelper.CheckFileExist(
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
            ClientConnection.CsHelper = _csHelper;//注入codesoft

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

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(Resources.SureToExit, Resources.SystemPromote,
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) 
                == DialogResult.Cancel)
                return;

            _server.DoCleaning();
            _csHelper.QuitCodeSoft();

            Logger.Glog.Info("服务器关闭");
            Logger.Glog.Stop();
            Application.Exit();
        }

    }
}
