using BoardAutoTesting.BLL;
using BoardAutoTesting.Log;
using BoardAutoTesting.Model;
using BoardAutoTesting.Properties;
using BoardAutoTesting.Status;

namespace BoardAutoTesting.DataExchange
{
    public class DoorOpen : BaseCommand, IAction
    {
        public DoorOpen(ClientConnection client) :
            base(client)
        {
        }

        public void ExecuteCommand(string command)
        {
            Client.IsOpenDoor = true;
            Client.SendMsg(CmdInfo.OpenGet);
            string craft;
            if (!UpdateLine(out craft))
            {
                //应该是不可能出现的情况
                Logger.Glog.Info(Client.ClientIp,
                "DoorOpen.ExecuteCommand.UpdateLine",
                Resources.UpdateError);
                return;
            }

            if (!UpdateOnLineProduct(craft))
            {
                //应该是不可能出现的情况
                Logger.Glog.Info(Client.ClientIp,
                "DoorOpen.ExecuteCommand.UpdateOnLineProduct",
                Resources.UpdateError);
                return;
            }

            if (!UpdateTestingProduct())
            {
                //应该是不可能出现的情况
                Logger.Glog.Info(Client.ClientIp,
                "DoorOpen.ExecuteCommand.UpdateTestingProduct",
                Resources.UpdateError);
                return;
            }

            Logger.Glog.Info(Client.ClientIp,
                "DoorOpen.ExecuteCommand", Resources.CommandExecuted);
        }

        private bool UpdateOnLineProduct(string craft)
        {
            ProductInfo product = ProductBll.GetModelByCraftStatus(
                craft, ProductAction.OnLine);
            if (product == null)
            {
                //应该是不可能出现的情况
                Logger.Glog.Info(Client.ClientIp,
                    "DoorOpen.UpdateOnLineProduct.GetProductInfoByIpStatus",
                    Resources.NoTestingProduct);
                return true;
            }

            product.IsPass = ProductStatus.Fail.ToString();
            product.ActionName = ProductAction.EndTest.ToString();
            return ProductBll.SureToUpdateModel(product);
        }

        private bool UpdateTestingProduct()
        {
            ProductInfo product = ProductBll.GetProductInfoByIpStatus(
                Client.ClientIp, ProductAction.Testing);
            if (product == null)
            {
                //应该是不可能出现的情况
                Logger.Glog.Info(Client.ClientIp,
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
            LineInfo line = LineBll.GetModelByIpPort(Client.ClientIp, "NA");
            craft = "";
            if (line == null)
            {
                //应该是不可能出现的情况
                Logger.Glog.Info(Client.ClientIp,
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