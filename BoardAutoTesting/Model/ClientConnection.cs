using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using BoardAutoTesting.DataExchange;
using BoardAutoTesting.Log;
using BoardAutoTesting.Service;
using BoardAutoTesting.Status;

namespace BoardAutoTesting.Model
{
    public class ClientConnection
    {
        #region Field

        private readonly Thread _threadClient;
        private Socket _socket;
        private bool _doesClose;
        private IAction _action;
        private string _rfid = "NA";

        #endregion

        #region Property

        public string ClientIp
        {
            get { return ((IPEndPoint) _socket.RemoteEndPoint).Address.ToString(); }
        }

        public string Rfid
        {
            get { return _rfid; }
            set { _rfid = value; }
        }

        public string SecondCommand { get; set; }
        public string FirstCommand { get; set; }
        public bool IsOpenDoor { get; set; }
        public static CodeSoftHelper CsHelper { get; set; }
        public static tCheckDataTestAteSoapClient Ate { get; set; }
        public static SystemInfo SysModel { get; set; }

        #endregion

        #region Constructor

        public ClientConnection(Socket socket)
        {
            _socket = socket;
            _threadClient = new Thread(WatchMsg) {IsBackground = true};
            _threadClient.Start();
            _doesClose = false;
        }

        public ClientConnection()
        {
        }

        #endregion

        private IAction CommandFactory(string cmd)
        {
            string command = cmd.Replace("*", "").Replace("#", "");
            if (command.Contains(":IN?"))
                command = command.Remove(0, 8);
            IAction action;
            switch (command)
            {
                case ":IN?":
                    action = new CanIn(this);
                    break;

                case "RESULT:PASS":
                case "RESULT:FAIL":
                    action = new ProductPassFail(this);
                    break;

                case "RESULT:RETEST":
                    action = new ReTest(this);
                    break;

                case "TEST:MAC?":
                    action = new TestMac(this);
                    break;

                case "Door:Open":
                    action = new DoorOpen(this);
                    break;

                case "Door:Close":
                    action = new DoorClose(this);
                    break;

                default:
                    action = null;
                    break;
            }

            return action;
        }

        public bool Analyse(string msg, out string command)
        {
            command = "";
            int startIndex = msg.IndexOf("*", StringComparison.Ordinal);
            int endIndex = msg.IndexOf("#", StringComparison.Ordinal);

            if (startIndex < 0 || endIndex < 0)
                return false;

            if (startIndex > endIndex)
                return false;

            int length = endIndex - startIndex + 1;
            if (length <= 2)
                return false;

            command = msg.Substring(startIndex, length);
            return true;
        }

        public bool GetSearchId(string cmd, string action, out string rfid)
        {
            rfid = cmd.Replace("*", "").Replace("#", "").Replace(action, "").Trim();
            if (rfid.Length == 8)
                return true;

            Logger.Glog.Info(ClientIp, "ClientConnection.GetSearchId",
                string.Format("{0} Wrong RFID:{1}", cmd, rfid));
            return false;
        }

        private void WatchMsg()
        {
            while (!_doesClose)
            {
                try
                {
                    byte[] byteMsgRec = new byte[1024];
                    int length = _socket.Receive(byteMsgRec, byteMsgRec.Length,
                        SocketFlags.None);
                    if (length <= 0) continue;
                    string strMsgRec = Encoding.UTF8.GetString(
                        byteMsgRec, 0, length);

                    Thread responseThread = new Thread(() =>
                    {
                        Response(strMsgRec);
                    })
                    {
                        IsBackground = true
                    };

                    responseThread.Start();
                }
                catch (Exception e)
                {
                    if (_socket != null)
                    {
                        string msg = string.Format("Client:{0} disconnects: {1}",
                            ClientIp, e.Message);
                        Logger.Glog.Info(msg);
                    }

                    break;
                }
            }
        }

        private void Response(string recvMsg)
        {
            string cmd;
            if (!Analyse(recvMsg, out cmd))
                return;

            //以下指令属于主动询问式指令，所以需要创建对应的类进行处理
            if (cmd.Contains(CmdInfo.CanIn) || cmd.Contains(CmdInfo.ResultPass) ||
                cmd.Contains(CmdInfo.ResultFail) || cmd.Contains(CmdInfo.ResultRetest) ||
                cmd.Contains(CmdInfo.TestMac) || cmd.Contains(CmdInfo.DoorOpen) ||
                cmd.Contains(CmdInfo.DoorClose))
            {
                _action = CommandFactory(cmd);
                if (_action != null)
                    _action.ExecuteCommand(cmd);
                else
                    Logger.Glog.Info(ClientIp, "ClientConnection.Response",
                        "无法解析的命令，工厂创建命令失败");
            }
            else //其他情况直接给全局变量赋值，相当于对消息进行分发
            {
                if (cmd.Contains("GET"))
                    FirstCommand = cmd;
                else
                    SecondCommand = cmd;
            }

        }

        public void Close()
        {
            _doesClose = true;
            _threadClient.Abort();
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
            _socket = null;
        }

        public void SendMsg(string msg)
        {
            try
            {
                byte[] msgSendByte = Encoding.UTF8.GetBytes(msg);
                _socket.Send(msgSendByte);
            }
            catch (Exception e)
            {
                Logger.Glog.Info(ClientIp, "ClientConnection.SendMsg", e.Message);
            }
        }
    }
}