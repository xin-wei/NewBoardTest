﻿using System.Collections.Generic;
using BoardAutoTesting.BLL;
using BoardAutoTesting.Log;
using BoardAutoTesting.Model;
using BoardAutoTesting.Properties;
using BoardAutoTesting.Status;

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

            McuClient.SendMsg(CmdInfo.GoInGet);

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
            if (ClientConnection.SysModel.IsLogin)
            {
                strResult = ClientConnection.Ate.Check_Route_ATE(product.ESN,
                    product.RouteName, ClientConnection.SysModel.WoId);
            }

            #region 最后一站特殊处理

            if (line.IsOut)
            {
                if (product.IsPass == ProductStatus.Pass.ToString()
                    && strResult == "OK")
                {
                    WaitGetResponse(CmdInfo.ProductPass, TimeOut, CmdInfo.ProductGet);
                }
                else
                {
                    WaitGetResponse(CmdInfo.ProductFail, TimeOut, CmdInfo.ProductGet);
                    ClientConnection.CsHelper.ClearVariables();
                    Dictionary<string, object> dicVariables = new Dictionary<string, object>
                    {
                        {"ESN", product.ESN},
                        {"CRAFT", product.CraftID},
                        {"EC", LineBll.GetRouteIpById(product.CraftID)}
                    };
                    ClientConnection.CsHelper.Fill_Label_Variables(dicVariables);
                    ClientConnection.CsHelper.PrintLabel();
                }

                ProductInfo originProductInfo = new ProductInfo()
                {
                    RFID = product.RFID,
                    ESN = "NA",
                    RouteName = "NA",
                    CraftID = "NA",
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

                Logger.Glog.Info(McuClient.ClientIp, "CanIn.UpdateModel",
                        Resources.Finish);
                return;
            }

            #endregion

            if (McuClient.Rfid == product.RFID)
                return;
            McuClient.Rfid = product.RFID;

            if (product.IsPass == ProductStatus.Fail.ToString() || strResult != "OK")
                return;

            if (line.RouteName != product.RouteName)
            {
                product.RouteName = line.RouteName;
                product.CraftID = "NA";
                product.ActionName = ProductAction.OnLine.ToString();
            }

            if (product.ActionName == ProductAction.EndTest.ToString())
            {
                NextStation(product);
                return;
            }

            if (product.CraftID == "NA") //产品机台状态为空，分配机台
            {
                string assignedCraft;
                LineBll.WaitAndOccupyCraft(McuClient, line.RouteName,
                    product.ESN, out assignedCraft);

                if (assignedCraft == "")
                {
                    //出现异常的概率很小，即使出现了造成卡板，板子拿掉就行
                    Logger.Glog.Info(McuClient.ClientIp,
                        "CanIn.ExecuteCommand.WaitAndOccupyCraft", 
                        "没有分配到机台怎么可能进来，估计抛异常了");
                    return;
                }

                product.CraftID = assignedCraft;
            }

            if (McuClient.IsOpenDoor)
                return;

            if (line.CraftId == product.CraftID && !line.IsRepair)
            {
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

            if (!WaitGetResponse(CmdInfo.GoIn, TimeOut, CmdInfo.GoInGet))
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

            LineInfo line = LineBll.GetModelByIpPort(McuClient.ClientIp, "NA");
            if (line == null)
            {
                //应该是不可能出现的问题
                Logger.Glog.Info(McuClient.ClientIp,
                    "CanIn.InStation.GetModelByIpPort",
                    Resources.UnconfigedCraft);
                return;
            }
            if (line.LineEsn == "")
                return;

            line.LineEsn = "";
            if (!LineBll.SureToUpdateModel(line, "Mcu_Ip"))
            {
                Logger.Glog.Info(McuClient.ClientIp, "CanIn.InStation.SureToUpdateModel",
                    Resources.UpdateError);
            }

            RedLedOnOrOff(false);
            Logger.Glog.Info(McuClient.ClientIp,
                "CanIn.InStation.SureToUpdateModel", "进站完成");
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

            if (!WaitGetResponse(CmdInfo.GoNext, TimeOut, CmdInfo.GoNextGet))
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

            LineInfo line = LineBll.GetModelByIpPort(McuClient.ClientIp, "NA");
            if (line == null)
            {
                //应该是不可能出现的问题
                Logger.Glog.Info(McuClient.ClientIp,
                    "CanIn.NextStation.GetModelByIpPort",
                    Resources.UnconfigedCraft);
                return;
            }
            if (line.LineEsn == "")
                return;

            line.LineEsn = "";
            if (!LineBll.SureToUpdateModel(line, "Mcu_Ip"))
            {
                //应该是不可能发生的
                Logger.Glog.Info(McuClient.ClientIp,
                    "CanIn.NextStation.SureToUpdateModel",
                    Resources.UpdateError);
            }

            Logger.Glog.Info(McuClient.ClientIp,
                "CanIn.NextStation.SureToUpdateModel", "过站完成");
        }

    }
}