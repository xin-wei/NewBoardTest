using System;
using BoardAutoTesting.Log;
using BoardAutoTesting.Model;
using BoardAutoTesting.Status;

namespace BoardAutoTesting.DataExchange
{
    public interface IAction
    {
        void ExecuteCommand(string command);
    }

    public class BaseCommand
    {
        protected const int SendInterval = 3000;
        protected ClientConnection Client;

        protected const string Pass = "Pass";
        protected const string Fail = "Fail";

        public BaseCommand(ClientConnection client)
        {
            Client = client;
        }

        protected void LightRedLed()
        {
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    Client.SendMsg(CmdInfo.RLightOn);
                }
                catch (Exception e)
                {
                    Logger.Glog.Info(Client.ClientIp + ":" + e.Message);
                }
            }
        }
    }
}