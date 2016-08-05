using System.Collections.Generic;
using System.Data;
using System.Linq;
using BoardAutoTesting.Model;
using BoardAutoTesting.Status;
using DataAccess;
using GenericProvider;

namespace BoardAutoTesting.DAL
{
    public class ProductDal
    {
        private const string TableName = "centercontrol.tb_product_info";

        private static IDictionary<string, object> GetModelDic(ProductInfo product)
        {
            IDictionary<string, object> mst = new Dictionary<string, object>();
            mst.Add("RFID", product.RFID);
            mst.Add("ESN", product.ESN);
            mst.Add("Is_Pass", product.IsPass);
            mst.Add("Route_Name", product.RouteName);
            mst.Add("Craft_Idx", product.CraftId);
            mst.Add("Current_IP", product.CurrentIp);
            mst.Add("Old_IP", product.OldIp);
            mst.Add("Action_Name", product.ActionName);
            mst.Add("ATE_IP", product.ATEIp);

            return mst;
        }

        private static ProductInfo ToModel(DataRow dr)
        {
            ProductInfo product = new ProductInfo
            {
                RFID = dr["RFID"].ToString(),
                ESN = dr["ESN"].ToString(),
                IsPass = dr["Is_Pass"].ToString(),
                RouteName = dr["Route_Name"].ToString(),
                CraftId = dr["Craft_Idx"].ToString(),
                CurrentIp = dr["Current_IP"].ToString(),
                OldIp = dr["Old_IP"].ToString(),
                ActionName = dr["Action_Name"].ToString(),
                ATEIp = dr["ATE_IP"].ToString()
            };

            return product;
        }

        public static void InsertModel(ProductInfo product)
        {
            IAdminProvider dp = (IAdminProvider) DpFactory.Create(typeof (IAdminProvider), DpFactory.ADMIN);
            IDictionary<string, object> mst = GetModelDic(product);
            dp.AddData(TableName, mst);
        }

        public static ProductInfo GetModelByIpStatus(string ip, ProductAction action)
        {
            IAdminProvider dp = (IAdminProvider) DpFactory.Create(typeof (IAdminProvider), DpFactory.ADMIN);
            string filter = string.Format("Current_IP = '{0}' and Action_Name = '{1}'",
                ip, action);
            int count;
            DataSet ds = dp.GetData(TableName, "*", filter, null, null, "", out count);
            if (count != 1)
                return null;

            DataTable dt = ds.Tables[0];
            DataRow dr = dt.Rows[0];

            return ToModel(dr);
        }

        public static ProductInfo GetModelByRfid(string id)
        {
            IAdminProvider dp = (IAdminProvider) DpFactory.Create(typeof (IAdminProvider), DpFactory.ADMIN);
            string filter = string.Format("RFID = '{0}'", id);
            int count;
            DataSet ds = dp.GetData(TableName, "*", filter, null, null, "", out count);
            if (count != 1)
                return null;

            DataTable dt = ds.Tables[0];
            DataRow dr = dt.Rows[0];

            return ToModel(dr);
        }

        public static ProductInfo GetModelByCraftStatus(string id, ProductAction action)
        {
            IAdminProvider dp = (IAdminProvider) DpFactory.Create(typeof (IAdminProvider), DpFactory.ADMIN);
            string filter = string.Format("Craft_Idx = '{0}' and Action_Name = '{1}'",
                id, action);
            int count;
            DataSet ds = dp.GetData(TableName, "*", filter, null, null, "", out count);
            if (count != 1)
                return null;

            DataTable dt = ds.Tables[0];
            DataRow dr = dt.Rows[0];

            return ToModel(dr);
        }

        public static List<ProductInfo> GetModels()
        {
            IAdminProvider dp = (IAdminProvider) DpFactory.Create(typeof (IAdminProvider), DpFactory.ADMIN);
            List<ProductInfo> lstInfos = new List<ProductInfo>();
            int count;
            DataSet ds = dp.GetData(TableName, "*", null, out count);
            if (count <= 0)
                return lstInfos;

            DataTable dt = ds.Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                lstInfos.Add(ToModel(dr));
            }

            return lstInfos;
        }

        public static void DeleteModel(ProductInfo product)
        {
            IAdminProvider dp = (IAdminProvider) DpFactory.Create(typeof (IAdminProvider), DpFactory.ADMIN);
            IDictionary<string, object> mst = new Dictionary<string, object>();
            mst.Add("RFID", product.RFID);

            dp.DeleteData(TableName, mst);
        }

        public static int UpdateModel(ProductInfo product)
        {
            IDictionary<string, object> mst = GetModelDic(product);
            string sql = mst.Keys.Aggregate("update " + TableName + " set ",
                (current, key) => current + (key + " = '" + mst[key] + "',"));
            sql = sql.Remove(sql.LastIndexOf(','));
            sql += string.Format(" where RFID = '{0}'", product.RFID);

            return MySqlHelper.ExecuteNonQuery(DbHelper.ConnectionStringProfile, CommandType.Text,
                sql, null);
        }


    }
}