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
                Logger.Glog.Info(Client.ClientIp, "ReTest.GetPortResult",
                    Resources.WrongCommand);
                return;
            }

            LineInfo line = LineBll.GetModelByIpPort(Client.ClientIp, port);
            if (line == null) //应该是不可能出现的情况
            {
                Logger.Glog.Info(Client.ClientIp, "ReTest.GetModelByIpPort",
                    "根据端口查询产品失败");
                return;
            }

            ProductInfo product = ProductBll.GetProductInfoByIpStatus(Client.ClientIp,
                ProductAction.Testing);
            if (product == null)
            {
                Logger.Glog.Info(Client.ClientIp, "ReTest.GetProductInfoByIpStatus",
                    "没有测试中的产品，无法复测");
                return;
            }

            if (!WaitGetResponse(CmdInfo.GoRetest, TimeOut, CmdInfo.GoRetestGet))
            {
                Logger.Glog.Info(Client.ClientIp, "ReTest.WaitGetResponse",
                    "没收到RepGet，单片机傻了");
                RedLedOnOrOff(true);
                return;
            }

            if (!WaitOkResponse(TimeOut, CmdInfo.GoRetestOk))
            {
                Logger.Glog.Info(Client.ClientIp, "ReTest.WaitOkResponse",
                    "没收到RepOk，单片机傻了，估计传感器问题");
                RedLedOnOrOff(true);
                return;
            }
            Client.SendMsg(CmdInfo.GoRetestOk);

            RedLedOnOrOff(false);

        }


    }
}