﻿using System;
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
        private readonly Dictionary<string, ClientConnection> _dictConnections = 
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
                    string msg = string.Format("客户端：{0}连接成功...",
                        ((IPEndPoint)cSocket.RemoteEndPoint).Address);
                    Logger.Glog.Info(msg);
                    _dictConnections.Add(cSocket.RemoteEndPoint.ToString(), conn);
                }
                catch (Exception e)
                {
                    Logger.Glog.Info("WatchPort():" + e.Message);
                    break;
                }
            }
        }

        private void ClearConnection()
        {
            foreach (string key in new List<string>(_dictConnections.Keys))
            {
                _dictConnections[key].Close();
            }
            _dictConnections.Clear();
        }

        public void DoCleaning()
        {
            _sockWatchPort.Close();
            _threadWatchPort.Abort();
            ClearConnection();
        }
    }
}