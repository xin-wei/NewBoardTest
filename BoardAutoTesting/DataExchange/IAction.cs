using System;
using System.Threading;
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
        protected const int ResendTimes = 5;
        protected readonly int TimeOut = SendInterval*ResendTimes;
        protected ClientConnection Client;

        public BaseCommand(ClientConnection client)
        {
            Client = client;
        }

        /// <summary>
        /// 传入true打开LED，反之关闭
        /// </summary>
        /// <param name="cmd"></param>
        protected void RedLedOnOrOff(bool cmd)
        {
            for (int i = 0; i < 3; i++)
            {
                Client.SendMsg(cmd ? CmdInfo.RLightOn : CmdInfo.RLightOff);
            }
        }

        /// <summary>
        /// 根据指令获取端口号，用于ATE一拖二
        /// </summary>
        /// <param name="cmd">指令内容</param>
        /// <param name="port">解析的端口号</param>
        protected bool GetPortResult(string cmd, out string port)
        {
            cmd = cmd.Replace("*", "").Replace("#", "");
            port = "NA";

#if _NEW_VERSION
            port = cmd.Substring(cmd.Length - 1, 1);
#endif

            return cmd.Contains("RESULT:PASS") || cmd.Contains("RESULT:FAIL") 
                || cmd.Contains("RESULT:RETEST") || cmd.Contains("TEST:MAC");
        }

        /// <summary>
        /// 等待单片机回复收到应答指令
        /// </summary>
        /// <param name="sendCmd">中控发送的指令</param>
        /// <param name="timeout">应答超时时间</param>
        /// <param name="expectedCmd">期待收到的指令</param>
        /// <returns>是否收到应答</returns>
        protected bool WaitGetResponse(string sendCmd, int timeout, string expectedCmd)
        {
            int startTick = Environment.TickCount;
            int endTick = Environment.TickCount;
            while (endTick - startTick < timeout)
            {
                Client.SendMsg(sendCmd);

                Thread.Sleep(SendInterval);
                if (Client.IsOpenDoor)
                    break;

                if (Client.Command == expectedCmd)
                    return true;

                endTick = Environment.TickCount;
            }

            return false;
        }

        /// <summary>
        /// 等待单片机回复动作Ok指令，一般nextok需要等待很久，所以传入无限等待时间
        /// </summary>
        /// <param name="timeout">超时时间</param>
        /// <param name="expectedCmd">中控期待收到的指令</param>
        /// <returns>是否在指定时间收到指令</returns>
        protected bool WaitOkResponse(int timeout, string expectedCmd)
        {
            int startTick = Environment.TickCount;
            int endTick = Environment.TickCount;
            while (endTick - startTick < timeout)
            {
                Thread.Sleep(SendInterval);
                if (Client.IsOpenDoor)
                    break;

                if (Client.Command == expectedCmd)
                    return true;

                endTick = Environment.TickCount;
            }

            return false;
        }

    }
}