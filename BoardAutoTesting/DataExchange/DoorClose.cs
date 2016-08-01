using BoardAutoTesting.BLL;
using BoardAutoTesting.Log;
using BoardAutoTesting.Model;
using BoardAutoTesting.Properties;
using BoardAutoTesting.Status;

namespace BoardAutoTesting.DataExchange
{
    public class DoorClose : BaseCommand, IAction
    {
        public DoorClose(ClientConnection client) 
            : base(client)
        {
        }

        public void ExecuteCommand(string command)
        {
            Client.SendMsg(CmdInfo.CloseGet);
            LineInfo line = LineBll.GetModelByIpPort(Client.ClientIp, command);
            if (line == null)
            {
                //应该是不可能出现的情况
                Logger.Glog.Info(Client.ClientIp,
                "DoorClose.ExecuteCommand.GetModelByIpPort",
                Resources.UnconfigedCraft);
                return;
            }
            line.IsRepair = false;
            if (!LineBll.SureToUpdateModel(line, "Mcu_Ip"))
            {
                //应该是不可能出现的情况
                Logger.Glog.Info(Client.ClientIp,
                "DoorClose.ExecuteCommand.SureToUpdateModel",
                Resources.UpdateError);
                return;
            }

            Logger.Glog.Info(Client.ClientIp,
                "DoorClose.ExecuteCommand", Resources.CommandExecuted);
        }
    }
}