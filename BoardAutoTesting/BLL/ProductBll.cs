using System.Collections.Generic;
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

        public static int UpdateModel(ProductInfo product)
        {
            return ProductDal.UpdateModel(product);
        }

        public static ProductInfo GetModelByCraftStatus(string id, ProductAction action)
        {
            return ProductDal.GetModelByCraftStatus(id, action);
        }
    }
}