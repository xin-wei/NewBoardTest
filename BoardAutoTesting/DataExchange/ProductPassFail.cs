using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BoardAutoTesting.BLL;
using BoardAutoTesting.Log;
using BoardAutoTesting.Model;
using BoardAutoTesting.Properties;
using BoardAutoTesting.Status;
using Commons;

namespace BoardAutoTesting.DataExchange
{
    public class ProductPassFail : BaseCommand, IAction
    {
        public ProductPassFail(ClientConnection client)
        {
            SetAteClient(client);
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
                Logger.Glog.Info(AteClient.ClientIp,
                    "ProductPassFail.ExecuteCommand.GetPortResult",
                    Resources.WrongCommand);
                return;
            }

            ProductInfo product;
            if (!OutPrepare(cmd, port, out product)) return;

            if (!SetMcuClient(product.CurrentIp))
            {
                Logger.Glog.Info(AteClient.ClientIp,
                    "ProductPassFail.OutExecuting.ContainsKey",
                    Resources.UnconfigedCraft);
                return;
            }

            LineInfo line;
            if (!OutExecuting(product, port, out line)) return;

            OutFinished(line);
            RedLedOnOrOff(false);
        }

        /// <summary>
        /// 出站前准备：获取产品信息，判断是否已测过
        /// 发生错误返回true
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="port"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        private bool OutPrepare(string cmd, string port, out ProductInfo product)
        {
            product = null;
            LineInfo line = LineBll.GetModelByIpPort(AteClient.ClientIp, port);
            if (line == null)
            {
                //应该是不可能出现的问题
                Logger.Glog.Info(AteClient.ClientIp,
                    "ProductPassFail.OutPrepare.GetModelByIpPort",
                    Resources.UnconfigedCraft);
                return false;
            }
            product = ProductBll.GetModelByIpStatus(line.McuIp,
                ProductAction.Testing);
            if (product == null)
            {
                //应该是不可能发生的
                Logger.Glog.Info(line.McuIp,
                    "ProductPassFail.OutPrepare.GetProductInfoByIpStatus",
                    Resources.UnconfigedCraft);
                return false;
            }

            if (product.IsPass != ProductStatus.UnKnown.ToString())
            {
                Logger.Glog.Info(AteClient.ClientIp,
                    "ProductPassFail.OutPrepare.GetProductInfoByIpStatus",
                    "测过了，别害我了！");
                AteClient.SendMsg(CmdInfo.OutPut);
                return false;
            }

            product.ActionName = ProductAction.EndTest.ToString();
            product.IsPass = cmd.Contains("RESULT:PASS")
                ? ProductStatus.Pass.ToString()
                : ProductStatus.Fail.ToString();

            if (!ProductBll.SureToUpdateModel(product))
            {
                //应该是不可能发生的
                Logger.Glog.Info(AteClient.ClientIp,
                    "ProductPassFail.OutPrepare.SureToUpdateModel",
                    Resources.UpdateError);
                return false;
            }

            INIFileUtil iniFile = new INIFileUtil(
                string.Format(@"{0}\result.ini", Application.StartupPath));
            string lastTime = iniFile.IniReadValue(Resources.Section, "Time");
            //应该是一定成立的
            if (DateTime.Parse(lastTime).DayOfYear == DateTime.Now.DayOfYear)
            {
                string result = iniFile.IniReadValue(Resources.Section, line.CraftId);
                string[] results = result.Split('/');
                int pass = int.Parse(results[0]);
                int fail = int.Parse(results[1]);
                if (cmd.Contains("RESULT:PASS"))
                    pass++;
                else
                    fail++;

                string newResult = pass + "/" + fail;
                iniFile.IniWriteValue(Resources.Section, line.CraftId, newResult);
            }

            AteClient.SendMsg(CmdInfo.OutPut); //应答ATE
            return true;
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
            LineBll.WaitAndOccupyLine(McuClient, product.ESN);
            if (McuClient.IsOpenDoor)
                return false;

            if (!WaitGetResponse(CmdInfo.GoOut, TimeOut, CmdInfo.GoOutGet))
            {
                Logger.Glog.Info(McuClient.ClientIp,
                    "ProductPassFail.OutExecuting.WaitGetResponse",
                    Resources.NoResponse);
                return false;
            }

            if (!WaitOkResponse(TimeOut, CmdInfo.GoOutOk))
            {
                if (McuClient.IsOpenDoor)
                    return false;

                Logger.Glog.Info(McuClient.ClientIp,
                    "ProductPassFail.OutExecuting.WaitOkResponse",
                    Resources.NoResponse);
                RedLedOnOrOff(true);
                return false;
            }
            McuClient.SendMsg(CmdInfo.GoOutOk); //应答单片机

            line = LineBll.GetModelByIpPort(AteClient.ClientIp, port);
            if (line == null)
            {
                //应该是不可能出现的问题
                Logger.Glog.Info(McuClient.ClientIp,
                    "ProductPassFail.OutExecuting.GetModelByIpPort",
                    Resources.UnconfigedCraft);
                return false;
            }
            if (line.CraftEsn == "")
            {
                //应该是不可能出现的问题
                Logger.Glog.Info(McuClient.ClientIp,
                    "ProductPassFail.OutExecuting.GetModelByIpPort",
                    "我都没更新你怎么就清空机台了，是个问题");
                return true;
            }
            line.CraftEsn = "";
            if (!LineBll.SureToUpdateModel(line, "Mcu_Ip"))
            {
                //除非网络太差，否则不可能出现
                Logger.Glog.Info(McuClient.ClientIp,
                    "ProductPassFail.OutExecuting.SureToUpdateModel",
                    Resources.UpdateError);
            }
            return true;
        }

        /// <summary>
        /// 过站动作：等待nextok--清空线体信息--关闭LED
        /// </summary>
        /// <param name="line"></param>
        private void OutFinished(LineInfo line)
        {
            if (!WaitOkResponse(int.MaxValue, CmdInfo.GoNextOk))
            {
                Logger.Glog.Info(McuClient.ClientIp,
                    "ProductPassFail.OutFinished.WaitOkResponse",
                    Resources.NoResponse);
                RedLedOnOrOff(true);
                return;
            }
            McuClient.SendMsg(CmdInfo.GoNextOk);

            if (line.LineEsn == "")
            {
                //应该是不可能出现的问题
                Logger.Glog.Info(McuClient.ClientIp,
                    "ProductPassFail.OutFinished.GetModelByIpPort",
                    "我都没更新你怎么就清空线体了，是个问题");
                return;
            }
            line.LineEsn = "";
            if (!LineBll.SureToUpdateModel(line, "Mcu_Ip"))
            {
                Logger.Glog.Info(McuClient.ClientIp, 
                    "ProductPassFail.OutFinished.SureToUpdateModel",
                    Resources.UpdateError);
            }
            
        }
        
    }
}