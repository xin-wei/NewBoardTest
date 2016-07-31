using System.Collections.Generic;
using BoardAutoTesting.BLL;
using BoardAutoTesting.Log;
using BoardAutoTesting.Model;
using BoardAutoTesting.Status;

namespace BoardAutoTesting.DataExchange
{
    public class CanIn : BaseCommand, IAction 
    {
        public CanIn(ClientConnection client)
            : base(client)
        {
        }

        public void ExecuteCommand(string cmd)
        {
            string rfid;
            bool result = Client.GetSearchId(cmd, ":IN?", out rfid);
            if (!result)
                return;

            Client.SendMsg(CmdInfo.GoInGet);

            ProductInfo product = ProductBll.GetModelByRfid(rfid);
            if (product == null)
            {
                Logger.Glog.Info(Client.ClientIp, "CanIn.GetModelByRfid",
                    string.Format("{0}:未查找到指定rfid的产品信息", cmd));
                return;
            }

            if (product.ESN == "NA" || string.IsNullOrEmpty(product.ESN))
            {
                Logger.Glog.Info(Client.ClientIp, "CanIn.GetModelByRfid",
                    string.Format("{0}:ESN不存在", cmd));
                return;
            }

            LineInfo line = LineBll.GetModelByIpPort(Client.ClientIp, "NA");
            if (line == null)
            {
                Logger.Glog.Info(Client.ClientIp, "CanIn.GetModelByIpPort",
                    string.Format("{0}:未查找到指定ip的线体信息", cmd));
                return;
            }

            //检测途程
            string strResult = "OK";
            //if ()

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

                if (ProductBll.UpdateModel(originProductInfo) != 1)//应该是不可能出现的
                    Logger.Glog.Info(Client.ClientIp, "CanIn.UpdateModel",
                        "最后一站更新数据出错");

                return;
            }

            #endregion

            if (Client.Rfid == product.RFID)
                return;
            Client.Rfid = product.RFID;

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
                LineBll.WaitAndOccupyCraft(Client, line.RouteName,
                    product.ESN, out assignedCraft);

                if (assignedCraft == "")
                {
                    //出现异常的概率很小，即使出现了造成卡板，板子拿掉就行
                    Logger.Glog.Info(Client.ClientIp, "CanIn.WaitAndOccupyCraft", 
                        "没有分配到机台怎么可能进来，估计抛异常了");
                    return;
                }

                product.CraftID = assignedCraft;
            }

            if (Client.IsOpenDoor)
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
            LineBll.WaitAndOccupyLine(Client, product.ESN);
            if (Client.IsOpenDoor)
                return;

            product.OldIp = product.CurrentIp;
            product.CurrentIp = Client.ClientIp;
            product.ActionName = ProductAction.Testing.ToString();
            product.IsPass = ProductStatus.UnKnown.ToString();
            if (ProductBll.UpdateModel(product) != 1)//应该是不可能发生的
                return;

            if (!WaitGetResponse(CmdInfo.GoIn, TimeOut, CmdInfo.GoInGet))
            {
                Logger.Glog.Info(Client.ClientIp, "CanIn.WaitGetResponse", "未收到进站应答");
                RedLedOnOrOff(true);
                return;
            }

            if (!WaitOkResponse(TimeOut, CmdInfo.GoInOk))
            {
                Logger.Glog.Info(Client.ClientIp, "CanIn.WaitOkResponse", "未收到IN:OK反馈");
                RedLedOnOrOff(true);
                return;
            }
            Client.SendMsg(CmdInfo.GoInOk);

            LineInfo line = LineBll.GetModelByIpPort(Client.ClientIp, "NA");
            if (line.LineEsn == "")
                return;

            line.LineEsn = "";
            if (!LineBll.SureToUpdateModel(line, "Mcu_Ip"))
                Logger.Glog.Info(Client.ClientIp, "CanIn.SureToUpdateModel",
                    "进站数据没更新成功");

            RedLedOnOrOff(false);
            Logger.Glog.Info(Client.ClientIp, "CanIn.SureToUpdateModel",
                    "进站完成");
        }

        private void NextStation(ProductInfo product)
        {
            LineBll.WaitAndOccupyLine(Client, product.ESN);
            if (Client.IsOpenDoor)
                return;

            product.OldIp = product.CurrentIp;
            product.CurrentIp = Client.ClientIp;
            product.ActionName = ProductAction.OnLine.ToString();
            if (ProductBll.UpdateModel(product) != 1) //应该是不可能发生的
                return;

            if (!WaitGetResponse(CmdInfo.GoNext, TimeOut, CmdInfo.GoNextGet))
            {
                Logger.Glog.Info(Client.ClientIp, "CanIn.WaitGetResponse", "未收到过站应答");
                RedLedOnOrOff(true);
                return;
            }

            if (!WaitOkResponse(int.MaxValue, CmdInfo.GoNextOk))
            {
                Logger.Glog.Info(Client.ClientIp, "CanIn.WaitOkResponse", "未收到过站反馈");
                RedLedOnOrOff(true);
                return;
            }
            Client.SendMsg(CmdInfo.GoNextOk);

            LineInfo line = LineBll.GetModelByIpPort(Client.ClientIp, "NA");
            if (line.LineEsn == "")
                return;

            line.LineEsn = "";
            if (!LineBll.SureToUpdateModel(line, "Mcu_Ip"))
                Logger.Glog.Info(Client.ClientIp, "CanIn.SureToUpdateModel",
                    "过站数据没更新成功");

            Logger.Glog.Info(Client.ClientIp, "CanIn.SureToUpdateModel",
                    "过站完成");
        }

    }
}