using System.Threading;
using BoardAutoTesting.BLL;
using BoardAutoTesting.Log;
using BoardAutoTesting.Model;
using BoardAutoTesting.Properties;
using BoardAutoTesting.Status;

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

            LineInfo line = LineBll.GetModelByIpPort(AteClient.ClientIp, port);
            if (line == null)
            {
                //应该是不可能出现的情况
                AteClient.SendMsg(AteClient.ClientIp);
                Logger.Glog.Info(AteClient.ClientIp,
                    "TestMac.ExecuteCommand.GetModelByIpPort",
                    Resources.UnconfigedCraft);
                return;
            }

            ProductInfo product1 = ProductBll.GetProductInfoByIpStatus(line.McuIp,
                ProductAction.Testing);
            if (product1 == null)
            {
                AteClient.SendMsg(AteClient.ClientIp);
                return;
            }

            Thread.Sleep(300);
            ProductInfo product2 = ProductBll.GetProductInfoByIpStatus(line.McuIp,
                ProductAction.Testing);
            if (product2 == null)
            {
                AteClient.SendMsg(AteClient.ClientIp);
                return;
            }

            AteClient.SendMsg(product2.ESN);
            Logger.Glog.Info(AteClient.ClientIp,
                "TestMac.ExecuteCommand",
                Resources.CommandExecuted + ":" + product2.ESN);
        }
    }
}