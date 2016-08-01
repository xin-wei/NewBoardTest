using System.Threading;
using BoardAutoTesting.BLL;
using BoardAutoTesting.Log;
using BoardAutoTesting.Model;
using BoardAutoTesting.Properties;

namespace BoardAutoTesting.DataExchange
{
    public class TestMac : BaseCommand, IAction
    {
        public TestMac(ClientConnection client)
            : base(client)
        {
        }

        public void ExecuteCommand(string command)
        {
            string port;
            if (!GetPortResult(command, out port))
            {
                Logger.Glog.Info(Client.ClientIp, "TestMac.GetPortResult",
                    Resources.WrongCommand);
                return;
            }

            LineInfo info1 = LineBll.GetModelByIpPort(Client.ClientIp, port);
            if (info1 == null)
            {
                Client.SendMsg(Client.ClientIp);
                Logger.Glog.Info(Client.ClientIp, "TestMac.GetModelByIpPort", 
                    "看到我说明连接有问题");
                return;
            }

            if (info1.CraftEsn == "")
            {
                Client.SendMsg(Client.ClientIp);
                return;
            }

            Thread.Sleep(400);
            LineInfo info2 = LineBll.GetModelByIpPort(Client.ClientIp, port);
            if (info2 == null)
            {
                Client.SendMsg(Client.ClientIp);
                Logger.Glog.Info(Client.ClientIp, "TestMac.GetModelByIpPort",
                    "睡过400ms后看到我说明连接有问题");
                return;
            }

            if (info2.CraftEsn == "")
            {
                Client.SendMsg(Client.ClientIp);
                return;
            }

            Client.SendMsg(info2.CraftEsn);
            Logger.Glog.Info(Client.ClientIp, "TestMac.ExecuteCommand",
                    "本站ESN为：" + info2.CraftEsn);
        }
    }
}