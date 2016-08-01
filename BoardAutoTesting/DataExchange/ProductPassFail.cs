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
                Logger.Glog.Info(Client.ClientIp, "ProductPassFail.GetPortResult",
                    Resources.WrongCommand);
                return;
            }

            ProductInfo product;
            if (OutPrepare(cmd, out product)) return;

            LineInfo line;
            if (OutExecuting(product, port, out line)) return;

            OutFinished(line);
        }

        /// <summary>
        /// 过站动作：等待nextok--清空线体信息--关闭LED
        /// </summary>
        /// <param name="line"></param>
        private void OutFinished(LineInfo line)
        {
            if (!WaitOkResponse(int.MaxValue, CmdInfo.GoNextOk))
            {
                Logger.Glog.Info(Client.ClientIp, "ProductPassFail.WaitOkResponse",
                    "未收到NextOk，卡死了");
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
                Logger.Glog.Info(Client.ClientIp, "CanIn.SureToUpdateModel",
                    "过站数据没更新成功");

            Logger.Glog.Info(Client.ClientIp, "CanIn.SureToUpdateModel",
                "过站完成");
            RedLedOnOrOff(false);
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
                Logger.Glog.Info(Client.ClientIp, "ProductPassFail.WaitGetResponse",
                    "没收到OutGet，单片机傻了");
                return true;
            }

            if (!WaitOkResponse(TimeOut, CmdInfo.GoOutOk))
            {
                if (Client.IsOpenDoor)
                    return true;

                Logger.Glog.Info(Client.ClientIp, "ProductPassFail.WaitOkResponse",
                    "没收到OutOk，传感器傻了");
                RedLedOnOrOff(true);
                return true;
            }
            Client.SendMsg(CmdInfo.GoOutOk); //应答单片机

            line = LineBll.GetModelByIpPort(Client.ClientIp, port);
            if (line.CraftEsn == "")
            {
                //应该是不可能出现的问题
                Logger.Glog.Info(Client.ClientIp, "ProductPassFail.GetModelByIpPort",
                    "我都没更新你怎么就清空机台了，是个问题");
            }
            line.CraftEsn = "";
            if (!LineBll.SureToUpdateModel(line, "Mcu_Ip"))
            {
                //除非网络太差，否则不可能出现
                Logger.Glog.Info(Client.ClientIp, "CanIn.SureToUpdateModel",
                    "出站数据没更新成功--机台未释放");
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
                Logger.Glog.Info(Client.ClientIp, "ProductPassFail.GetProductInfoByIpStatus",
                    "怎么可能查不到！");
                return true;
            }

            if (product.IsPass != ProductStatus.UnKnown.ToString())
            {
                Logger.Glog.Info(Client.ClientIp, "ProductPassFail.GetProductInfoByIpStatus",
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
                Logger.Glog.Info(Client.ClientIp, "ProductPassFail.SureToUpdateModel",
                    "并发访问不行啊，都更新不了！");
                return true;
            }

            Client.SendMsg(CmdInfo.OutPut); //应答ATE
            return false;
        }
    }
}