using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using BoardAutoTesting.Log;
using BoardAutoTesting.Model;

namespace BoardAutoTesting.DataExchange
{
    public class CenterServer
    {
        private IPAddress _address;
        private IPEndPoint _endPoint;

        private Thread _threadWatchPort;
        private Socket _sockWatchPort;

        public static readonly Dictionary<string, ClientConnection> DictConnections = 
            new Dictionary<string, ClientConnection>();

        private const int MaxWatchPort = 20;

        public void WatchConnection(string ip, int port)
        {
            try
            {
                _address = IPAddress.Parse(ip);
                _endPoint = new IPEndPoint(_address, port);
                _sockWatchPort = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);
                _sockWatchPort.Bind(_endPoint);
                _sockWatchPort.Listen(MaxWatchPort);

                _threadWatchPort = new Thread(WatchPort)
                {
                    Name = "threadWatchPort",
                    IsBackground = true
                };
                _threadWatchPort.Start();
                Logger.Glog.Info("-->监听线程启动成功，等待客户端连接");
            }
            catch (Exception e)
            {
                Logger.Glog.Info(e.ToString());
            }
        }

        private void WatchPort()
        {
            while (true)
            {
                try
                {
                    Socket cSocket = _sockWatchPort.Accept();
                    ClientConnection conn = new ClientConnection(cSocket);
                    string ip = ((IPEndPoint)cSocket.RemoteEndPoint).Address.ToString();

                    if (DictConnections.ContainsKey(ip))
                        DictConnections[ip] = conn;
                    else
                        DictConnections.Add(ip, conn);
                }
                catch (Exception e)
                {
                    Logger.Glog.Info("WatchPort():" + e.Message);
                }
            }
            // ReSharper disable once FunctionNeverReturns
        }

        private void ClearConnection()
        {
            foreach (string key in new List<string>(DictConnections.Keys))
            {
                DictConnections[key].Close();
            }
            DictConnections.Clear();
        }

        public void DoCleaning()
        {
            if (_sockWatchPort != null)
                _sockWatchPort.Close();

            if (_threadWatchPort != null)
                _threadWatchPort.Abort();

            ClearConnection();
        }
    }
}