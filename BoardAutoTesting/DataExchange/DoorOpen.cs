using BoardAutoTesting.BLL;
using BoardAutoTesting.Log;
using BoardAutoTesting.Model;
using BoardAutoTesting.Properties;
using BoardAutoTesting.Status;

namespace BoardAutoTesting.DataExchange
{
    public class DoorOpen : BaseCommand, IAction
    {
        public DoorOpen(ClientConnection client)
        {
            SetMcuClient(client);
        }

        public void ExecuteCommand(string command)
        {
            McuClient.IsOpenDoor = true;
            McuClient.SendMsg(CmdInfo.OpenGet);
            string craft;
            if (!UpdateLine(out craft))
            {
                //应该是不可能出现的情况
                Logger.Glog.Info(McuClient.ClientIp,
                    "DoorOpen.ExecuteCommand.UpdateLine",
                    Resources.UpdateError);
                return;
            }

            if (!UpdateOnLineProduct(craft))
            {
                //应该是不可能出现的情况
                Logger.Glog.Info(McuClient.ClientIp,
                    "DoorOpen.ExecuteCommand.UpdateOnLineProduct",
                    Resources.UpdateError);
                return;
            }

            if (!UpdateTestingProduct())
            {
                //应该是不可能出现的情况
                Logger.Glog.Info(McuClient.ClientIp,
                    "DoorOpen.ExecuteCommand.UpdateTestingProduct",
                    Resources.UpdateError);
                return;
            }

            Logger.Glog.Info(McuClient.ClientIp,
                "DoorOpen.ExecuteCommand",
                Resources.CommandExecuted);
        }

        private bool UpdateOnLineProduct(string craft)
        {
            ProductInfo product = ProductBll.GetModelByCraftStatus(
                craft, ProductAction.OnLine);
            if (product == null)
            {
                Logger.Glog.Info(McuClient.ClientIp,
                    "DoorOpen.UpdateOnLineProduct.GetProductInfoByIpStatus",
                    Resources.NoOnLineProduct);
                return true;
            }

            product.IsPass = ProductStatus.Fail.ToString();
            product.ActionName = ProductAction.EndTest.ToString();
            return ProductBll.SureToUpdateModel(product);
        }

        private bool UpdateTestingProduct()
        {
            ProductInfo product = ProductBll.GetModelByIpStatus(
                McuClient.ClientIp, ProductAction.Testing);
            if (product == null)
            {
                Logger.Glog.Info(McuClient.ClientIp,
                    "DoorOpen.UpdateTestingProduct.GetProductInfoByIpStatus",
                    Resources.NoTestingProduct);
                return true;
            }

            product.IsPass = ProductStatus.Fail.ToString();
            product.ActionName = ProductAction.EndTest.ToString();
            return ProductBll.SureToUpdateModel(product);
        }

        private bool UpdateLine(out string craft)
        {
            LineInfo line = LineBll.GetModelByIpPort(McuClient.ClientIp, "NA");
            craft = "";
            if (line == null)
            {
                //应该是不可能出现的情况
                Logger.Glog.Info(McuClient.ClientIp,
                    "DoorOpen.UpdateLine.GetModelByIpPort",
                    Resources.UnconfigedCraft);
                return false;
            }
            craft = line.CraftId;
            line.IsRepair = true;
            line.CraftEsn = "";
            line.LineEsn = "";
            return LineBll.SureToUpdateModel(line, "Mcu_Ip");
        }
    }
}