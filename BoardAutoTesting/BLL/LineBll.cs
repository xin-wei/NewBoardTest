using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using BoardAutoTesting.DAL;
using BoardAutoTesting.Log;
using BoardAutoTesting.Model;

namespace BoardAutoTesting.BLL
{
    public class LineBll
    {
        public static void InsertModel(LineInfo line)
        {
            LineDal.InsertModel(line);
        }

        public static LineInfo GetMaxModel()
        {
            return LineDal.GetMaxModel();
        }

        public static List<LineInfo> GetModels()
        {
            return LineDal.GetModels();
        }

        public static string GetRouteIpById(string id)
        {
            LineInfo info = LineDal.GetModelById(id);
            if (info == null)
                return "NA";

            string strRoute = info.RouteName.Replace("PCBA_", "").Replace("ASSY_", "");
            string strMcu = info.McuIp;
            string strTemp = strMcu.Substring(strMcu.LastIndexOf('.'));

            return strRoute + "/" + strTemp;
        }

        public static LineInfo GetModelByIpPort(string ip, string port)
        {
#if _NEW_VERSION
            return LineDal.GetModelByIpPort(ip, port);
#else
            return LineDal.GetModelByIpPort(ip, "NA");
#endif
            
        }

        public static LineInfo GetModelByRouteEmptyCraft(string route)
        {
            List<LineInfo> lstInfos = LineDal.GetModelByRouteEmptyCraft(route);
            return lstInfos == null ? null : lstInfos[0];
        }

        private static int UpdateModel(LineInfo line, string condition)
        {
            return LineDal.UpdateModel(line, condition);
        }

        public static bool SureToUpdateModel(LineInfo line, string condition)
        {
            int startTick = Environment.TickCount;
            int endTick = Environment.TickCount;

            while (endTick - startTick < 3000)
            {
                if (UpdateModel(line, condition) == 1)
                    return true;

                Thread.Sleep(300);
                endTick = Environment.TickCount;
            }

            return false;
        }

        /// <summary>
        /// 根据途程名称检测该途程上有没有空机台
        /// </summary>
        /// <param name="route">指定途程</param>
        /// <returns>分配的机台Id</returns>
        private static string GetEmptyCraft(string route)
        {
            try
            {
                List<LineInfo> lstInfos = LineDal.GetModelByRouteEmptyCraft(route);
                if (lstInfos == null || lstInfos.Count < 1) return "";
                return lstInfos[0].CraftId;
            }
            catch (Exception e)
            {
                Logger.Glog.Info(e.Message);
                return "";
            }
        }

        public static void WaitAndOccupyCraft(ClientConnection client, 
            string strRoute, string occupationEsn, out string givenCraft)
        {
            givenCraft = "";
            Logger.Glog.Info(client.ClientIp, "WaitAndOccupyCraft" + occupationEsn,
                "开始抢夺机台");
            try
            {
                while (true)
                {
                    if (client.IsOpenDoor)
                        break;

                    givenCraft = GetEmptyCraft(strRoute);
                    if (givenCraft == "")
                    {
                        Thread.Sleep(300);
                        continue;
                    }

                    if (LineDal.UpdateCraftStatus(occupationEsn, strRoute) == 1)
                        break;
                }

                Logger.Glog.Info(client.ClientIp, "LineBll.WaitAndOccupyCraft", givenCraft);
            }
            catch (Exception e)
            {
                Logger.Glog.Info(client.ClientIp,
                    "LineBll.WaitAndOccupyCraft.Exception", e.Message);
            }
        }

        /// <summary>
        /// 抢夺线体资源，没抢到就等待，抢到退出
        /// </summary>
        /// <param name="client">线体所在的客户端</param>
        /// <param name="occupationEsn">抢到线体的板子</param>
        public static void WaitAndOccupyLine(ClientConnection client, string occupationEsn)
        {
            Logger.Glog.Info(client.ClientIp, "WaitAndOccupyLine:" + occupationEsn, 
                "单片机开始抢夺线体");
            try
            {
                while (true)
                {
                    if (client.IsOpenDoor)
                        break;

                    if (LineDal.UpdateLineStatus(client.ClientIp, occupationEsn) == 1)
                        break;

                    Thread.Sleep(300);
                }

                Logger.Glog.Info(client.ClientIp, "WaitAndOccupyLine:" + occupationEsn, "OK");
            }
            catch (Exception e)
            {
                Logger.Glog.Info(client.ClientIp,
                    "LineBll.WaitAndOccupyLine.Exception", e.Message);
            }
        }

        public static void DeleteModel(LineInfo line)
        {
            LineDal.DeleteModel(line);
        }
    }
}