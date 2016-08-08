using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BoardAutoTesting.DAL;
using BoardAutoTesting.Model;
using BoardAutoTesting.Status;

namespace BoardAutoTesting.BLL
{
    public class ProductBll
    {
        public static void InsertModel(ProductInfo product)
        {
            ProductDal.InsertModel(product);
        }

        public static ProductInfo GetModelByIpStatus(string ip, ProductAction action)
        {
            List<ProductInfo> lstProducts = ProductDal.GetModelByIpStatus(ip, action);
            if (lstProducts == null)
                return null;

            if (lstProducts.Count == 1)
                return lstProducts[0];

            return (from t in lstProducts
                    let line = LineBll.GetModelByIpPort(t.CurrentIp, "NA")
                    where line.CraftEsn == t.ESN
                    select t).FirstOrDefault();
        }

        public static ProductInfo GetModelByRfid(string id)
        {
            return ProductDal.GetModelByRfid(id);
        }

        public static List<ProductInfo> GetModels()
        {
            return ProductDal.GetModels();
        }

        public static void DeleteModel(ProductInfo product)
        {
            ProductDal.DeleteModel(product);
        }

        private static int UpdateModel(ProductInfo product)
        {
            return ProductDal.UpdateModel(product);
        }

        public static bool SureToUpdateModel(ProductInfo product)
        {
            int startTick = Environment.TickCount;
            int endTick = Environment.TickCount;

            while (endTick - startTick < 3000)
            {
                if (UpdateModel(product) == 1)
                    return true;

                Thread.Sleep(300);
                endTick = Environment.TickCount;
            }

            return false;
        }

        /// <summary>
        /// 根据机台状态获取产品信息
        /// 为了防止数据库中垃圾信息的干扰，对查询到的信息做了核对
        /// </summary>
        /// <param name="id">机台id</param>
        /// <param name="action">产品动作</param>
        /// <returns>查询到的产品信息</returns>
        public static ProductInfo GetModelByCraftStatus(string id, ProductAction action)
        {
            List<ProductInfo> lstProducts = ProductDal.GetModelByCraftStatus(id, action);
            if (lstProducts == null)
                return null;

            if (lstProducts.Count == 1)
                return lstProducts[0];

            return (from t in lstProducts 
                    let line = LineBll.GetModelByIpPort(t.CurrentIp, "NA") 
                    where line.CraftEsn == t.ESN 
                    select t).FirstOrDefault();
        }
    }
}