using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using BoardAutoTesting.BLL;
using BoardAutoTesting.Log;
using BoardAutoTesting.Model;
using BoardAutoTesting.Properties;
using BoardAutoTesting.Status;
using Commons;

namespace BoardAutoTesting.DataExchange
{
    public class CanIn : BaseCommand, IAction
    {
        public CanIn(ClientConnection client)
        {
            SetMcuClient(client);
        }

        public void ExecuteCommand(string cmd)
        {
            string rfid;
            bool result = McuClient.GetSearchId(cmd, ":IN?", out rfid);
            if (!result)
                return;

            ProductInfo product = ProductBll.GetModelByRfid(rfid);
            if (product == null)
            {
                Logger.Glog.Info(McuClient.ClientIp, "CanIn.ExecuteCommand.GetModelByRfid",
                    string.Format("{0}:未查找到指定rfid的产品信息", cmd));
                return;
            }

            if (product.ESN == "NA" || string.IsNullOrEmpty(product.ESN))
            {
                Logger.Glog.Info(McuClient.ClientIp, "CanIn.ExecuteCommand.GetModelByRfid",
                    string.Format("{0}:ESN不存在", cmd));
                return;
            }

            LineInfo line = LineBll.GetModelByIpPort(McuClient.ClientIp, "NA");
            if (line == null)
            {
                Logger.Glog.Info(McuClient.ClientIp,
                    "CanIn.ExecuteCommand.GetModelByIpPort",
                    Resources.UnconfigedCraft);
                return;
            }

            //检测途程
            string strResult = "OK";
            if (ClientConnection.SysModel != null && ClientConnection.SysModel.IsLogin)
            {
                strResult = ClientConnection.Ate.Check_Route_ATE(product.ESN,
                    line.RouteName, ClientConnection.SysModel.WoId);
            }

            if (strResult != "OK" && !AllRoutes.LstRoutes.Contains(strResult))
            {
                Logger.Glog.Info(McuClient.ClientIp,
                    "CanIn.ExecuteCommand.AllRoutes",
                    "产品不属于任何途程，怎么可能进来？");
                RedLedOnOrOff(true);
                return;
            }
            RedLedOnOrOff(false);

            //不用急着回复，反正一条指令没执行完，其他指令也不会响应
            McuClient.SendMsg(CmdInfo.GoInGet);

            #region 最后一站特殊处理

            if (line.IsOut)
            {
                //翻天的时候更新记录
                INIFileUtil iniFile = new INIFileUtil(
                    string.Format(@"{0}\result.ini", Application.StartupPath));
                string lastTime = iniFile.IniReadValue(Resources.Section, "Time");
                if (DateTime.Parse(lastTime).DayOfYear != DateTime.Now.DayOfYear)
                {
                    iniFile.IniWriteValue(Resources.Section, "Time",
                        DateTime.Now.ToString(CultureInfo.InvariantCulture));
                    List<LineInfo> lineInfos = LineBll.GetModels();
                    foreach (var newLine in lineInfos)
                    {
                        iniFile.IniWriteValue(Resources.Section, newLine.CraftId, "0/0");
                    }
                }

                string strLast = iniFile.IniReadValue(Resources.Section, line.CraftId);
                string[] results = strLast.Split('/');
                int pass = int.Parse(results[0]);
                int fail = int.Parse(results[1]);

                if (product.IsPass == ProductStatus.Pass.ToString() && strResult == "OK")
                {
                    pass++;
                    WaitGetResponse(CmdInfo.ProductPass, CmdInfo.ProductGet);
                }
                else
                {
                    fail++;
                    WaitGetResponse(CmdInfo.ProductFail, CmdInfo.ProductGet);
                    ClientConnection.CsHelper.ClearVariables();
                    Dictionary<string, object> dicVariables = new Dictionary<string, object>
                    {
                        {"ESN", product.ESN},
                        {"CRAFT", product.CraftId},
                        {"EC", LineBll.GetRouteIpById(product.CraftId)}
                    };
                    ClientConnection.CsHelper.Fill_Label_Variables(dicVariables);
                    ClientConnection.CsHelper.PrintLabel();
                }

                string strNew = pass + "/" + fail;
                iniFile.IniWriteValue(Resources.Section, line.CraftId, strNew);
                ProductInfo originProductInfo = new ProductInfo
                {
                    RFID = product.RFID,
                    ESN = "NA",
                    RouteName = "NA",
                    CraftId = "NA",
                    IsPass = ProductStatus.UnKnown.ToString(),
                    CurrentIp = "NA",
                    OldIp = "NA",
                    ActionName = ProductAction.OnLine.ToString(),
                    ATEIp = "NA"
                };

                if (!ProductBll.SureToUpdateModel(originProductInfo))
                {
                    //应该是不可能出现的
                    Logger.Glog.Info(McuClient.ClientIp, "CanIn.UpdateModel",
                        Resources.UpdateError);
                }

                return;
            }

            #endregion

            McuClient.Rfid = product.RFID;

            if (product.IsPass == ProductStatus.Fail.ToString())
            {
                NextStation(product);
                return;
            }

            if (strResult != "OK" && AllRoutes.LstRoutes.Contains(strResult))
            {
                Logger.Glog.Info(McuClient.ClientIp,
                    "CanIn.ExecuteCommand.AllRoutes",
                    "不属于当前站，过");
                NextStation(product);
                return;
            }

            if (line.RouteName != product.RouteName)
            {
                product.RouteName = line.RouteName;
                product.CraftId = "NA";
                product.ActionName = ProductAction.OnLine.ToString();
            }

            if (product.ActionName == ProductAction.EndTest.ToString())
            {
                NextStation(product);
                return;
            }

            if (product.CraftId == "NA") //产品机台状态为空，分配机台
            {
                string assignedCraft;
                LineBll.WaitAndOccupyCraft(McuClient, line.RouteName,
                    product.ESN, out assignedCraft);

                if (assignedCraft == "")
                {
                    //出现异常的概率很小，即使出现了造成卡板，板子拿掉就行
                    Logger.Glog.Info(McuClient.ClientIp,
                        "CanIn.ExecuteCommand.WaitAndOccupyCraft",
                        "没有分配到机台怎么可能进来");
                    return;
                }

                product.CraftId = assignedCraft;
            }

            if (McuClient.IsOpenDoor)
                return;

            if (line.CraftId == product.CraftId && !line.IsRepair)
            {
                Logger.Glog.Info(McuClient.ClientIp,
                    "CanIn.ExecuteCommand",
                    product.ESN + "，准备进站：" + product.CraftId);
                InStation(product);
            }
            else
            {
                NextStation(product);
            }

        }

        private void InStation(ProductInfo product)
        {
            LineBll.WaitAndOccupyLine(McuClient, product.ESN);
            if (McuClient.IsOpenDoor)
                return;

            product.OldIp = product.CurrentIp;
            product.CurrentIp = McuClient.ClientIp;
            product.ActionName = ProductAction.Testing.ToString();
            product.IsPass = ProductStatus.UnKnown.ToString();
            if (!ProductBll.SureToUpdateModel(product))
            {
                //应该是不可能发生的
                Logger.Glog.Info(McuClient.ClientIp,
                    "CanIn.InStation.SureToUpdateModel",
                    Resources.UpdateError);
                return;
            }

            if (!WaitGetResponse(CmdInfo.GoIn, CmdInfo.GoInGet))
            {
                Logger.Glog.Info(McuClient.ClientIp,
                    "CanIn.InStation.WaitGetResponse",
                    Resources.NoResponse);
                RedLedOnOrOff(true);
                return;
            }

            if (!WaitOkResponse(TimeOut, CmdInfo.GoInOk))
            {
                Logger.Glog.Info(McuClient.ClientIp,
                    "CanIn.InStation.WaitOkResponse",
                    Resources.NoResponse);
                RedLedOnOrOff(true);
                return;
            }
            McuClient.SendMsg(CmdInfo.GoInOk);

            if (!EmptyLineStatus())
                return;

            RedLedOnOrOff(false);
        }

        /// <summary>
        /// 更新线体
        /// </summary>
        /// <returns>线体状态是否更新成功</returns>
        private bool EmptyLineStatus()
        {
            LineInfo line = LineBll.GetModelByIpPort(McuClient.ClientIp, "NA");
            if (line == null)
            {
                //应该是不可能出现的问题
                Logger.Glog.Info(McuClient.ClientIp,
                    "CanIn.EmptyLineStatus.GetModelByIpPort",
                    Resources.UnconfigedCraft);
                return false;
            }
            if (line.LineEsn == "")
            {
                Logger.Glog.Info(McuClient.ClientIp,
                    "CanIn.EmptyLineStatus.GetModelByIpPort",
                    "没更新怎么就清空线体了，是个问题");
                return true;
            }

            line.LineEsn = "";
            if (LineBll.SureToUpdateModel(line, "Mcu_Ip")) return true;

            Logger.Glog.Info(McuClient.ClientIp, "CanIn.EmptyLineStatus.SureToUpdateModel",
                Resources.UpdateError);
            return false;
        }

        private void NextStation(ProductInfo product)
        {
            LineBll.WaitAndOccupyLine(McuClient, product.ESN);
            if (McuClient.IsOpenDoor)
                return;

            product.OldIp = product.CurrentIp;
            product.CurrentIp = McuClient.ClientIp;
            product.ActionName = ProductAction.OnLine.ToString();
            if (!ProductBll.SureToUpdateModel(product)) //应该是不可能发生的
            {
                //应该是不可能发生的
                Logger.Glog.Info(McuClient.ClientIp,
                    "CanIn.NextStation.SureToUpdateModel",
                    Resources.UpdateError);
                return;
            }

            if (!WaitGetResponse(CmdInfo.GoNext, CmdInfo.GoNextGet))
            {
                Logger.Glog.Info(McuClient.ClientIp,
                    "CanIn.NextStation.WaitGetResponse",
                    Resources.NoResponse);
                RedLedOnOrOff(true);
                return;
            }

            if (!WaitOkResponse(int.MaxValue, CmdInfo.GoNextOk))
            {
                Logger.Glog.Info(McuClient.ClientIp,
                    "CanIn.NextStation.WaitOkResponse",
                    Resources.NoResponse);
                RedLedOnOrOff(true);
                return;
            }
            McuClient.SendMsg(CmdInfo.GoNextOk);

            if (!EmptyLineStatus())
                return;

            RedLedOnOrOff(false);
        }

    }
}