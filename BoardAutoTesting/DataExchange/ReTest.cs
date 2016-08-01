using BoardAutoTesting.BLL;
using BoardAutoTesting.Log;
using BoardAutoTesting.Model;
using BoardAutoTesting.Properties;
using BoardAutoTesting.Status;

namespace BoardAutoTesting.DataExchange
{
    public class ReTest : BaseCommand, IAction
    {
        public ReTest(ClientConnection client) : 
            base(client)
        {
        }

        /// <summary>
        /// 以后的Retest指令格式是*RESULT:RETEST:1#或者*RESULT:RETEST:2#
        /// </summary>
        /// <param name="cmd"></param>
        public void ExecuteCommand(string cmd)
        {
            Client.SendMsg(CmdInfo.OutPut);

            string port;
            if (!GetPortResult(cmd, out port))
            {
                Logger.Glog.Info(Client.ClientIp, 
                    "ReTest.ExecuteCommand.GetPortResult",
                    Resources.WrongCommand);
                return;
            }

            LineInfo line = LineBll.GetModelByIpPort(Client.ClientIp, port);
            if (line == null) //应该是不可能出现的情况
            {
                Logger.Glog.Info(Client.ClientIp, 
                    "ReTest.ExecuteCommand.GetModelByIpPort",
                    Resources.UnconfigedCraft);
                return;
            }

            ProductInfo product = ProductBll.GetProductInfoByIpStatus(Client.ClientIp,
                ProductAction.Testing);
            if (product == null)
            {
                Logger.Glog.Info(Client.ClientIp,
                    "ReTest.ExecuteCommand.GetProductInfoByIpStatus",
                    Resources.NoTestingProduct);
                return;
            }

            if (!WaitGetResponse(CmdInfo.GoRetest, TimeOut, CmdInfo.GoRetestGet))
            {
                Logger.Glog.Info(Client.ClientIp,
                    "ReTest.ExecuteCommand.WaitGetResponse",
                    Resources.NoResponse);
                RedLedOnOrOff(true);
                return;
            }

            if (!WaitOkResponse(TimeOut, CmdInfo.GoRetestOk))
            {
                Logger.Glog.Info(Client.ClientIp,
                    "ReTest.ExecuteCommand.WaitOkResponse",
                    Resources.NoResponse);
                RedLedOnOrOff(true);
                return;
            }

            Client.SendMsg(CmdInfo.GoRetestOk);
            RedLedOnOrOff(false);
            Logger.Glog.Info(Client.ClientIp,
                "ReTest.ExecuteCommand", Resources.CommandExecuted);
        }


    }
}