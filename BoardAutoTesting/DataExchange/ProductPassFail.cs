using BoardAutoTesting.BLL;
using BoardAutoTesting.Log;
using BoardAutoTesting.Model;
using BoardAutoTesting.Properties;
using BoardAutoTesting.Status;

namespace BoardAutoTesting.DataExchange
{
    public class ProductPassFail : BaseCommand, IAction
    {
        public ProductPassFail(ClientConnection client)
            : base(client)
        {
        }

        /// <summary>
        /// PASS/FAIL后出站，应答ATE--让单片机执行出站--等待outok--等待--nextok
        /// 以后的Pass/Fail指令格式是*RESULT:PASS:1#或者*RESULT:PASS:2#
        /// </summary>
        /// <param name="cmd"></param>
        public void ExecuteCommand(string cmd)
        {
            string port;
            if (!GetPortResult(cmd, out port))
            {
                Logger.Glog.Info(Client.ClientIp,
                    "ProductPassFail.ExecuteCommand.GetPortResult",
                    Resources.WrongCommand);
                return;
            }

            ProductInfo product;
            if (OutPrepare(cmd, out product)) return;

            LineInfo line;
            if (OutExecuting(product, port, out line)) return;

            OutFinished(line);

            Logger.Glog.Info(Client.ClientIp,
                "ProductPassFail.OutFinished.SureToUpdateModel",
                Resources.CommandExecuted);
            RedLedOnOrOff(false);
        }

        /// <summary>
        /// 过站动作：等待nextok--清空线体信息--关闭LED
        /// </summary>
        /// <param name="line"></param>
        private void OutFinished(LineInfo line)
        {
            if (!WaitOkResponse(int.MaxValue, CmdInfo.GoNextOk))
            {
                Logger.Glog.Info(Client.ClientIp,
                    "ProductPassFail.OutFinished.WaitOkResponse",
                    Resources.NoResponse);
                RedLedOnOrOff(true);
                return;
            }
            Client.SendMsg(CmdInfo.GoNextOk);

            if (line.LineEsn == "")
            {
                //应该是不可能出现的问题
                Logger.Glog.Info(Client.ClientIp, "ProductPassFail.GetModelByIpPort",
                    "我都没更新你怎么就清空线体了，是个问题");
                return;
            }
            line.LineEsn = "";
            if (!LineBll.SureToUpdateModel(line, "Mcu_Ip"))
            {
                Logger.Glog.Info(Client.ClientIp, 
                    "ProductPassFail.OutFinished.SureToUpdateModel",
                    Resources.UpdateError);
            }
            
        }

        /// <summary>
        /// 出站动作：占用线体--发送出站指令--等待出站完成--更新线体信息
        /// 查询到错误状态返回true
        /// </summary>
        /// <param name="product"></param>
        /// <param name="port"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        private bool OutExecuting(ProductInfo product, string port, out LineInfo line)
        {
            line = null;
            LineBll.WaitAndOccupyLine(Client, product.ESN);
            if (Client.IsOpenDoor)
                return true;

            if (!WaitGetResponse(CmdInfo.GoOut, TimeOut, CmdInfo.GoOutGet))
            {
                Logger.Glog.Info(Client.ClientIp,
                    "ProductPassFail.OutExecuting.WaitGetResponse",
                    Resources.NoResponse);
                return true;
            }

            if (!WaitOkResponse(TimeOut, CmdInfo.GoOutOk))
            {
                if (Client.IsOpenDoor)
                    return true;

                Logger.Glog.Info(Client.ClientIp,
                    "ProductPassFail.OutExecuting.WaitOkResponse",
                    Resources.NoResponse);
                RedLedOnOrOff(true);
                return true;
            }
            Client.SendMsg(CmdInfo.GoOutOk); //应答单片机

            line = LineBll.GetModelByIpPort(Client.ClientIp, port);
            if (line == null)
            {
                //应该是不可能出现的问题
                Logger.Glog.Info(Client.ClientIp,
                    "ProductPassFail.OutExecuting.GetModelByIpPort",
                    Resources.UnconfigedCraft);
                return true;
            }
            if (line.CraftEsn == "")
            {
                //应该是不可能出现的问题
                Logger.Glog.Info(Client.ClientIp,
                    "ProductPassFail.OutExecuting.GetModelByIpPort",
                    "我都没更新你怎么就清空机台了，是个问题");
            }
            line.CraftEsn = "";
            if (!LineBll.SureToUpdateModel(line, "Mcu_Ip"))
            {
                //除非网络太差，否则不可能出现
                Logger.Glog.Info(Client.ClientIp,
                    "ProductPassFail.OutExecuting.SureToUpdateModel",
                    Resources.UpdateError);
            }
            return false;
        }

        /// <summary>
        /// 出站前准备：获取产品信息，判断是否已测过
        /// 发生错误返回true
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        private bool OutPrepare(string cmd, out ProductInfo product)
        {
            product = ProductBll.GetProductInfoByIpStatus(Client.ClientIp,
                ProductAction.Testing);
            if (product == null)
            {
                //应该是不可能发生的
                Logger.Glog.Info(Client.ClientIp,
                    "ProductPassFail.OutPrepare.GetProductInfoByIpStatus",
                    Resources.UnconfigedCraft);
                return true;
            }

            if (product.IsPass != ProductStatus.UnKnown.ToString())
            {
                Logger.Glog.Info(Client.ClientIp,
                    "ProductPassFail.OutPrepare.GetProductInfoByIpStatus",
                    "测过了，别害我了！");
                Client.SendMsg(CmdInfo.OutPut);
                return true;
            }

            product.ActionName = ProductAction.EndTest.ToString();
            product.IsPass = cmd.Contains("RESULT:PASS")
                ? ProductStatus.Pass.ToString()
                : ProductStatus.Fail.ToString();

            if (!ProductBll.SureToUpdateModel(product))
            {
                //应该是不可能发生的
                Logger.Glog.Info(Client.ClientIp,
                    "ProductPassFail.OutPrepare.SureToUpdateModel",
                    Resources.UpdateError);
                return true;
            }

            Client.SendMsg(CmdInfo.OutPut); //应答ATE
            return false;
        }
    }
}