using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using BoardAutoTesting.DataExchange;
using BoardAutoTesting.Log;
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
        private readonly Assembly _assembly = Assembly.GetExecutingAssembly();
        private string _rfid = "NA";

        #endregion

        #region Property

        public string ClientIp
        {
            get { return ((IPEndPoint)_socket.RemoteEndPoint).Address.ToString(); }
        }

        public string Rfid
        {
            get { return _rfid; }
            set { _rfid = value; }
        }

        public string Command { get; set; }
        public bool IsOpenDoor { get; set; }
        public static CodeSoftHelper CsHelper { get; set; }

        #endregion

        public ClientConnection(Socket socket)
        {
            _socket = socket;
            _threadClient = new Thread(WatchMsg) {IsBackground = true};
            _threadClient.Start();
        }

        public ClientConnection()
        {
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

            Logger.Glog.Info(ClientIp, cmd, string.Format("Wrong RFID:{0}", rfid));
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
            if (cmd.Contains(CmdInfo.CanIn) || cmd.Contains(CmdInfo.ProductPass) ||
                cmd.Contains(CmdInfo.ProductFail) || cmd.Contains(CmdInfo.ResultRetest))
            {
                /*_action = (IAction)_assembly.CreateInstance("CanIn");
                if (_action != null)
                    _action.ExecuteCommand(this, cmd);*/
            }
            else//其他情况直接给全局变量赋值，相当于对消息进行分发
            {
                Command = cmd;
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
                Logger.Glog.Info("SendMsg(string msg):" + e.Message);
            }
        }
    }
}