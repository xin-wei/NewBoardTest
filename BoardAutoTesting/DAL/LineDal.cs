﻿using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BoardAutoTesting.Model;
using BoardAutoTesting.Status;
using DataAccess;
using GenericProvider;

namespace BoardAutoTesting.DAL
{
    public class LineDal
    {
        private const string TableName = "board.tb_line_info";
        private static IAdminProvider dp = (IAdminProvider)DpFactory.Create(typeof(IAdminProvider), DpFactory.ADMIN);

        private static IDictionary<string, object> GetModelDic(LineInfo line)
        {
            IDictionary<string, object> mst = new Dictionary<string, object>();
            mst.Add("Craft_Idx", line.CraftId);
            mst.Add("Route_Name", line.RouteName);
            mst.Add("Line_Idx", line.LineIdx);
            mst.Add("Mcu_Ip", line.McuIp);
            mst.Add("Ate_Ip", line.AteIp);
            mst.Add("Is_Repair", line.IsRepair);
            mst.Add("Is_Out", line.IsOut);
            mst.Add("Line_ESN", line.LineEsn);
            mst.Add("Craft_ESN", line.CraftEsn);
            mst.Add("Port_Id", line.PortId);

            return mst;
        }

        private static LineInfo ToModel(DataRow dr)
        {
            LineInfo info = new LineInfo
            {
                CraftId = dr["Craft_Idx"].ToString(),
                RouteName = dr["Route_Name"].ToString(),
                LineIdx = dr["Line_Idx"].ToString(),
                McuIp = dr["Mcu_Ip"].ToString(),
                AteIp = dr["Ate_Ip"].ToString(),
                IsRepair = (bool)dr["Is_Repair"],
                IsOut = (bool)dr["Is_Out"],
                LineEsn = dr["Line_ESN"].ToString(),
                CraftEsn = dr["Craft_ESN"].ToString(),
                PortId = dr["Port_Id"].ToString(),
            };

            return info;
        }

        public static void InsertModel(LineInfo line)
        {
            IDictionary<string, object> mst = GetModelDic(line);
            dp.AddData(TableName, mst);
        }

        public static List<LineInfo> GetModelByRouteEmptyCraft(string route)
        {
            List<LineInfo> lstInfos = new List<LineInfo>();
            string filter = string.Format("Route_Name = '{0}' and Craft_ESN = '{1}' and Is_Repair = '0'", route, "");
            IList<OrderKey> lstOrder = new List<OrderKey>();
            OrderKey order = new OrderKey
            {
                IsAsc = false,
                KeyName = "Craft_Idx"
            };
            lstOrder.Add(order);

            int count;
            DataSet ds = dp.GetData(TableName, "*", filter, null, lstOrder, "", out count);
            if (count <= 0)
                return null;

            DataTable dt = ds.Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                lstInfos.Add(ToModel(dr));
            }

            return lstInfos;
        }

        /// <summary>
        /// 根据Ip和端口获取Model，MCU查询时port为NA，ate查询时为端口号
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns>线体Model</returns>
        public static LineInfo GetModelByIpPort(string ip, string port)
        {
            string filter;
            if (port == "NA")
            {
                filter = string.Format("Mcu_Ip = '{0}'", ip);
            }
            else
            {
                filter = string.Format("Ate_Ip = '{0}' and Port_Id = '{1}'",
                    ip, port);
            }

            int count;
            DataSet ds = dp.GetData(TableName, "*", filter, null, null, "", out count);
            if (count != 1)
                return null;

            DataTable dt = ds.Tables[0];
            DataRow dr = dt.Rows[0];

            return ToModel(dr);
        }

        public static LineInfo GetModelById(string id)
        {
            string filter = string.Format("Craft_Idx = '{0}'", id);

            int count;
            DataSet ds = dp.GetData(TableName, "*", filter, null, null, "", out count);
            if (count != 1)
                return null;

            DataTable dt = ds.Tables[0];
            DataRow dr = dt.Rows[0];

            return ToModel(dr);
        }

        public static List<LineInfo> GetModels()
        {
            List<LineInfo> lstInfos = new List<LineInfo>();
            int count;
            IAdminProvider dp = (IAdminProvider)DpFactory.Create(typeof(IAdminProvider), DpFactory.ADMIN);
            DataSet ds = dp.GetData(TableName, "*", null, out count);
            if (count <= 0) return lstInfos;

            DataTable dt = ds.Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                lstInfos.Add(ToModel(dr));
            }

            return lstInfos;
        }

        public static void DeleteModel(LineInfo line)
        {
            IDictionary<string, object> mst = new Dictionary<string, object>();
            mst.Add("Craft_Idx", line.CraftId);

            dp.DeleteData(TableName, mst);
        }

        /// <summary>
        /// 智慧的结晶啊！！！IT封装的MySql库把更新部分干掉了，现在恢复原貌！
        /// </summary>
        /// <param name="line">线体对象</param>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public static int UpdateModel(LineInfo line, string condition)
        {
            IDictionary<string, object> mst = GetModelDic(line);
            string sql = mst.Keys.Aggregate("update " + TableName + " set ",
                (current, key) => current + (key + " = '" + mst[key] + "',"));
            sql = sql.Remove(sql.LastIndexOf(','));
            sql += string.Format(" where {0} = '{1}'", condition, mst[condition]);

            return MySqlHelper.ExecuteNonQuery(DbHelper.ConnectionStringProfile, 
                CommandType.Text, sql, null);
        }

        /// <summary>
        /// 使用类似存储过程查询并更新线体状态，防止多线程抢占资源时冲突
        /// </summary>
        /// <param name="ip">当前站的mcu_ip</param>
        /// <param name="esn">占用线体的产品esn</param>
        /// <returns>没可用产品的话返回0，反之返回受影响行数，正常为1</returns>
        public static int UpdateLineStatus(string ip, string esn)
        {
            string sql =
                string.Format(
                    "update {0} set Line_ESN = '{1}' where Mcu_Ip = (select Mcu_Ip from {0} where Line_ESN = '' and Mcu_Ip = '{2}')",
                    TableName, esn, ip);
            return MySqlHelper.ExecuteNonQuery(DbHelper.ConnectionStringProfile,
                CommandType.Text, sql, null);
        }
    }
}