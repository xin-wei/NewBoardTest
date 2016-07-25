using BoardAutoTesting.BLL;
using BoardAutoTesting.Log;
using BoardAutoTesting.Model;
using BoardAutoTesting.Status;
using Model;

namespace BoardAutoTesting.DataExchange
{
    public class ProductPass : BaseCommand, IAction
    {
        public ProductPass(ClientConnection client)
            : base(client)
        {
        }

        public void ExecuteCommand(string command)
        {
            Client.SendMsg(CmdInfo.OutPut);
            ExecutePassFail(command);
        }

        //以后的Pass/Fail指令格式是*RESULT:PASS:1#或者*RESULT:PASS:2#
        private void ExecutePassFail(string cmd)
        {
            string port;
            bool result;
            GetPortResult(cmd, out result, out port);
            LineInfo line = LineBll.GetModelByIpPort(Client.ClientIp, port);
            if (line == null)//应该是不可能出现的情况
            {
                Logger.Glog.Info(Client.ClientIp, "ExecutePassFail", "根据端口查询产品失败");
                return;
            }


            ProductInfo product = ProductBll.GetProductInfoByIpStatus(Client.ClientIp,
                ProductStatus.Testing);
        }

        private void GetPortResult(string cmd, out bool result, out string port)
        {
            result = false;
            port = "1";

            if (cmd.Contains("RESULT:PASS") && cmd.Length == 15)
            {
                result = true;
                port = cmd.Substring(14, 1);
                return;
            }

            if (!cmd.Contains("RESULT:FAIL") || cmd.Length != 15) return;
            port = cmd.Substring(14, 1);
        }
    }
}