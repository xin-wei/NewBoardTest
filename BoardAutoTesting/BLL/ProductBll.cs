using System;
using System.Collections.Generic;
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

        public static ProductInfo GetProductInfoByIpStatus(string ip, ProductAction action)
        {
            return ProductDal.GetModelByIpStatus(ip, action);
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

        public static ProductInfo GetModelByCraftStatus(string id, ProductAction action)
        {
            return ProductDal.GetModelByCraftStatus(id, action);
        }
    }
}