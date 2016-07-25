using System;
using System.Collections.Generic;
using System.Threading;
using BoardAutoTesting.BLL;
using BoardAutoTesting.Log;
using BoardAutoTesting.Model;
using BoardAutoTesting.Status;
using Model;

namespace BoardAutoTesting.DataExchange
{
    public class CanIn : BaseCommand, IAction 
    {
        public CanIn(ClientConnection client)
            : base(client)
        {
        }

        private bool ExecuteAteResult(ProductInfo product, int timeout)
        {
            int startTick = Environment.TickCount;
            int endTick = Environment.TickCount;
            while (endTick - startTick < timeout)
            {
                try
                {
                    Client.SendMsg(product.IsPass == Pass
                        ? CmdInfo.ProductPass
                        : CmdInfo.ProductFail);
                }
                catch (Exception e)
                {
                    Logger.Glog.Info(Client.ClientIp + ":" + e.Message);
                }

                Thread.Sleep(SendInterval);
                if (Client.Command == CmdInfo.ProductGet ||
                    Client.IsOpenDoor)
                    break;

                endTick = Environment.TickCount;
            }

            return Client.Command == CmdInfo.ProductGet;
        }

        private bool ExecuteInOrNext(bool isIn, int timeout)
        {
            int startTick = Environment.TickCount;
            int endTick = Environment.TickCount;
            while (endTick - startTick < timeout)
            {
                try
                {
                    Client.SendMsg(isIn ? CmdInfo.GoIn : CmdInfo.GoNext);
                }
                catch (Exception e)
                {
                    Logger.Glog.Info(Client.ClientIp + ":" + e.Message);     
                }
                Thread.Sleep(SendInterval);
                if (Client.IsOpenDoor)
                    break;

                if (isIn && Client.Command == CmdInfo.GoInGet)
                    return true;

                if (!isIn && Client.Command == CmdInfo.GoNextGet)
                    return true;

                endTick = Environment.TickCount;
            }

            return false;
        }

        private bool ExecuteInOrNextOk(bool isIn, int timeout)
        {
            int startTick = Environment.TickCount;
            int endTick = Environment.TickCount;
            while (endTick - startTick < timeout)
            {
                try
                {
                    Client.SendMsg(isIn ? CmdInfo.GoInOk : CmdInfo.GoNextOk);
                }
                catch (Exception e)
                {
                    Logger.Glog.Info(Client.ClientIp + ":" + e.Message);
                }
                Thread.Sleep(SendInterval);
                if (Client.IsOpenDoor)
                    break;

                if (isIn && Client.Command == CmdInfo.GoInOk)
                {
                    Client.SendMsg(CmdInfo.GoInOk);
                    return true;
                }

                if (!isIn && Client.Command == CmdInfo.GoNextOk)
                {
                    Client.SendMsg(CmdInfo.GoNextOk);
                    return true;
                }

                endTick = Environment.TickCount;
            }

            return false;
        }

        public void ExecuteCommand(string cmd)
        {
            string rfid;
            bool result = Client.GetSearchId(cmd, CmdInfo.CanIn, out rfid);
            if (!result)
                return;

            Client.SendMsg(CmdInfo.GoInGet);

            ProductInfo product = ProductBll.GetModelByRfid(rfid);
            if (product == null)
            {
                Logger.Glog.Info(Client.ClientIp, cmd,
                    "未查找到指定rfid的产品信息");
                return;
            }

            LineInfo line = LineBll.GetModelByIpPort(Client.ClientIp, "NA");
            if (line == null)
            {
                Logger.Glog.Info(Client.ClientIp, cmd,
                    "未查找到指定ip的线体信息");
                return;
            }

            if (product.ESN == "NA" || string.IsNullOrEmpty(product.ESN))
            {
                Logger.Glog.Info(Client.ClientIp, cmd,
                    "ESN不存在");
                return;
            }

            //检测途程
            string strResult = "OK";
            //if ()

            if (line.IsOut)
            {
                if (product.IsPass == Pass
                    && strResult == "OK")
                {
                    ExecuteAteResult(product, 15000);
                }
                else
                {
                    ExecuteAteResult(product, 15000);
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
            }

            if (Client.Rfid == product.RFID)
                return;

            Client.Rfid = product.RFID;

            if (product.IsPass == Fail || strResult != "OK")
                return;

            if (line.RouteName != product.RouteName)
            {
                product.RouteName = line.RouteName;
                product.CraftID = "NA";
                product.ActionName = ProductStatus.OnLine.ToString();
            }

            if (product.ActionName == ProductStatus.EndTest.ToString())
            {
                NextOrInStation(product, false);
            }

            string assignedCraft = "NA";
            if (product.CraftID == "NA") //产品机台状态为空，分配机台
            {
                assignedCraft = LineBll.GetEmptyCraft(line.RouteName);
            }

            if (Client.IsOpenDoor)
                return;

            if (line.CraftId == assignedCraft && !line.IsRepair)
            {
                NextOrInStation(product, true);
            }
            else
            {
                NextOrInStation(product, false);
            }

        }

        /// <summary>
        /// 执行过站或进站动作动作
        /// </summary>
        /// <param name="product">产品实例</param>
        /// <param name="isIn">进站为true</param>
        private void NextOrInStation(ProductInfo product, bool isIn)
        {
            LineBll.WaitAndOccupyLine(Client, product.ESN);
            if (Client.IsOpenDoor)
                return;

            product.OldIp = product.CurrentIp;
            product.CurrentIp = Client.ClientIp;
            int count = ProductBll.UpdateModel(product);
            if (count != 1)//应该是不可能发生的
                return;

            if (!ExecuteInOrNext(isIn, 15000))
            {
                Logger.Glog.Info(Client.ClientIp, "NextOrInStation", "未收到应答");
                LightRedLed();
                return;
            }

            if (ExecuteInOrNextOk(isIn, 15000)) return;

            Logger.Glog.Info(Client.ClientIp, "NextOrInStation", "未收到到位反馈");
            LightRedLed();
        }

    }
}