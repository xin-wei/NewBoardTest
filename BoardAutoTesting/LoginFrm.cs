using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.ServiceModel;
using System.Windows.Forms;
using BoardAutoTesting.Model;
using BoardAutoTesting.Service;
using BoardAutoTesting.Status;
using BoardAutoTesting.UserInfo;
using Commons;
using DevComponents.DotNetBar;

namespace BoardAutoTesting
{
    public partial class LoginFrm : Office2007Form
    {
        public LoginFrm()
        {
            InitializeComponent();
        }

        private tCheckDataTestAteSoapClient _ate = new tCheckDataTestAteSoapClient();
        private tUserInfoSoapClient _userInfo = new tUserInfoSoapClient();
        private SystemInfo _model = new SystemInfo();
        private string _startUp, _configPath;

        private void LoginFrm_Load(object sender, EventArgs e)
        {
            _startUp = Application.StartupPath;
            _configPath = _startUp + @"\config.bin";

            GetConfig(_configPath);

            BasicHttpBinding binding = new BasicHttpBinding
            {
                MaxReceivedMessageSize = int.MaxValue,
                MaxBufferPoolSize = int.MaxValue,
                MaxBufferSize = int.MaxValue
            };

            //"http://170.1.1.231/SFIS_WEBSER_TEST/tCheckDataTestAte.asmx";//
            string remoteAddress =  _ate.Endpoint.Address.Uri.ToString();
            EndpointAddress endpoint = new EndpointAddress(remoteAddress);
            _ate = new tCheckDataTestAteSoapClient(binding, endpoint);

            // "http://170.1.1.231/SFIS_WEBSER_TEST/tUserinfo.asmx";
            remoteAddress = _userInfo.Endpoint.Address.Uri.ToString();
            endpoint = new EndpointAddress(remoteAddress);
            _userInfo = new tUserInfoSoapClient(binding, endpoint);

            //加载线体
            cbxLines.Items.Clear();
            cbxLines.Items.Add("-请选择-");
            foreach (var item in _ate.Get_Line_List())
            {
                cbxLines.Items.Add(item);
            }

            txtUserId.Focus();
        }

        private void GetConfig(string path)
        {
            if (!File.Exists(path)) return;

            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream(path,
                FileMode.Open, FileAccess.Read))
            {
                _model = (SystemInfo) formatter.Deserialize(stream);
                stream.Close();
            }

            systemInfoBindingSource.Add(_model);
        }

        private void SaveConfig()
        {
            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream(_configPath, 
                FileMode.Create, FileAccess.Write))
            {
                formatter.Serialize(stream, _model);
                stream.Close();
            }
        }

        private bool Login()
        {
            try
            {
                if (string.IsNullOrEmpty(txtUserId.Text))
                    throw new Exception("工号不能为空");
                if (string.IsNullOrEmpty(txtPwd.Text))
                    throw new Exception("密码不能为空");
                /*加载用户信息*/
                DataTable dtUserInfo = ReleaseData.arrByteToDataTable(
                    _userInfo.GetUserInfoByUserId(txtUserId.Text));
                if (dtUserInfo == null || dtUserInfo.Rows.Count < 1 ||
                    dtUserInfo.Rows[0]["pwd"].ToString() != txtPwd.Text)
                    throw new Exception("账号或密码有误");

                _model.IsLogin = Convert.ToBoolean(
                    int.Parse(dtUserInfo.Rows[0]["USERSTATUS"].ToString()));
                _model.LineId = cbxLines.SelectedItem.ToString();
                _model.UserId = dtUserInfo.Rows[0]["USERID"].ToString();
                _model.UserPwd = dtUserInfo.Rows[0]["PWD"].ToString();
                _model.UserName = dtUserInfo.Rows[0]["USERNAME"].ToString();
                _model.WoId = txtWorkOrder.Text;
                if (!_model.IsLogin)
                    throw new Exception("该账号已经停用");

                /*加载工单及料号信息*/
                DataSet dsWoInfo = _ate.GetWoInfoByWo(txtWorkOrder.Text);
                if (dsWoInfo == null)
                    throw new Exception("工单号有误");
                if (dsWoInfo.Tables[0].Rows.Count <= 0)
                    throw new Exception("工单号有误");
                DataSet dsProDuct = _ate.GetProductByPartNumber(
                    dsWoInfo.Tables[0].Rows[0]["PARTNUMBER"].ToString());
                if (dsProDuct == null)
                    throw new Exception("工单料号有误");
                if (dsProDuct.Tables[0].Rows.Count <= 0)
                    throw new Exception("工单料号有误");

                Debug.WriteLine("Loing:工单料号");
                _model.PartNumber = dsWoInfo.Tables[0].Rows[0]["PARTNUMBER"].ToString();
                _model.PartName = dsProDuct.Tables[0].Rows[0]["PRODUCTNAME"].ToString();

                SaveConfig();
                return true;
            }
            catch (Exception ex)
            {
                MessageUtil.ShowError(ex.Message);
                return false;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (Login())
            {
                DialogResult = DialogResult.Yes;
            }
        }

        private void txtPwd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            if (!string.IsNullOrEmpty(txtPwd.Text))
                txtWorkOrder.Focus();
        }

        private void txtUserId_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Convert.ToChar(e.KeyChar.ToString().ToUpper());
        }

        private void txtUserId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            if (!string.IsNullOrEmpty(txtUserId.Text))
                txtPwd.Focus();
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}