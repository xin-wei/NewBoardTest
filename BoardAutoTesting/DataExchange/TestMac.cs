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
        {
            SetAteClient(client);
        }

        public void ExecuteCommand(string command)
        {
            string port;
            if (!GetPortResult(command, out port))
            {
                Logger.Glog.Info(AteClient.ClientIp,
                    "TestMac.ExecuteCommand.GetPortResult",
                    Resources.WrongCommand);
                return;
            }

            LineInfo info1 = LineBll.GetModelByIpPort(AteClient.ClientIp, port);
            if (info1 == null)
            {
                //应该是不可能出现的情况
                AteClient.SendMsg(AteClient.ClientIp);
                Logger.Glog.Info(AteClient.ClientIp,
                    "TestMac.ExecuteCommand.GetModelByIpPort", 
                    Resources.UnconfigedCraft);
                return;
            }

            if (info1.CraftEsn == "")
            {
                AteClient.SendMsg(AteClient.ClientIp);
                return;
            }

            Thread.Sleep(400);
            LineInfo info2 = LineBll.GetModelByIpPort(AteClient.ClientIp, port);
            if (info2 == null)
            {
                AteClient.SendMsg(AteClient.ClientIp);
                Logger.Glog.Info(AteClient.ClientIp,
                    "TestMac.ExecuteCommand.GetModelByIpPort",
                    Resources.UnconfigedCraft);
                return;
            }

            if (info2.CraftEsn == "")
            {
                AteClient.SendMsg(AteClient.ClientIp);
                return;
            }

            AteClient.SendMsg(info2.CraftEsn);
            Logger.Glog.Info(AteClient.ClientIp,
                "TestMac.ExecuteCommand",
                Resources.CommandExecuted + ":" + info2.CraftEsn);
        }
    }
}