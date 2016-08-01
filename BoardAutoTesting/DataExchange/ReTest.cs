using BoardAutoTesting.BLL;
using BoardAutoTesting.Log;
using BoardAutoTesting.Model;
using BoardAutoTesting.Properties;
using BoardAutoTesting.Status;

namespace BoardAutoTesting.DataExchange
{
    public class ReTest : BaseCommand, IAction
    {
        public ReTest(ClientConnection client)
        {
            SetAteClient(client);
        }

        /// <summary>
        /// 以后的Retest指令格式是*RESULT:RETEST:1#或者*RESULT:RETEST:2#
        /// </summary>
        /// <param name="cmd"></param>
        public void ExecuteCommand(string cmd)
        {
            AteClient.SendMsg(CmdInfo.OutPut);

            string port;
            if (!GetPortResult(cmd, out port))
            {
                Logger.Glog.Info(AteClient.ClientIp, 
                    "ReTest.ExecuteCommand.GetPortResult",
                    Resources.WrongCommand);
                return;
            }

            LineInfo line = LineBll.GetModelByIpPort(AteClient.ClientIp, port);
            if (line == null) //应该是不可能出现的情况
            {
                Logger.Glog.Info(AteClient.ClientIp, 
                    "ReTest.ExecuteCommand.GetModelByIpPort",
                    Resources.UnconfigedCraft);
                return;
            }

            ProductInfo product = ProductBll.GetProductInfoByIpStatus(line.McuIp,
                ProductAction.Testing);
            if (product == null)
            {
                Logger.Glog.Info(line.McuIp,
                    "ReTest.ExecuteCommand.GetProductInfoByIpStatus",
                    Resources.NoTestingProduct);
                return;
            }

            if (!SetMcuClient(product.CurrentIp))
            {
                Logger.Glog.Info(AteClient.ClientIp,
                    "ReTest.ExecuteCommand.SetMcuClient",
                    Resources.UnconfigedCraft);
                return;
            }

            if (!WaitGetResponse(CmdInfo.GoRetest, TimeOut, CmdInfo.GoRetestGet))
            {
                Logger.Glog.Info(McuClient.ClientIp,
                    "ReTest.ExecuteCommand.WaitGetResponse",
                    Resources.NoResponse);
                RedLedOnOrOff(true);
                return;
            }

            if (!WaitOkResponse(TimeOut, CmdInfo.GoRetestOk))
            {
                Logger.Glog.Info(McuClient.ClientIp,
                    "ReTest.ExecuteCommand.WaitOkResponse",
                    Resources.NoResponse);
                RedLedOnOrOff(true);
                return;
            }

            McuClient.SendMsg(CmdInfo.GoRetestOk);
            RedLedOnOrOff(false);
            Logger.Glog.Info(McuClient.ClientIp,
                "ReTest.ExecuteCommand", Resources.CommandExecuted);
        }


    }
}