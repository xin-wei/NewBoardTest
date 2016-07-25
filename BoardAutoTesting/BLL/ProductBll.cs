using System.Collections.Generic;
using BoardAutoTesting.DAL;
using BoardAutoTesting.Status;
using Model;

namespace BoardAutoTesting.BLL
{
    public class ProductBll
    {
        public static void InsertModel(ProductInfo product)
        {
            ProductDal.InsertModel(product);
        }

        public static ProductInfo GetProductInfoByIpStatus(string ip, ProductStatus status)
        {
            return ProductDal.GetModelByIpStatus(ip, status);
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

        public static ProductInfo GetModelByCraftStatus(string id, ProductStatus status)
        {
            return ProductDal.GetModelByCraftStatus(id, status);
        }
    }
}