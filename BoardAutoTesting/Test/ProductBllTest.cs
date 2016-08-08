using BoardAutoTesting.BLL;
using BoardAutoTesting.Model;
using BoardAutoTesting.Status;
using NUnit.Framework;

namespace BoardAutoTesting.Test
{
    [TestFixture]
    public class ProductBllTest
    {
        [Test]
        public void GetModelByIpStatus_Get_ReturnsModel()
        {
            ProductInfo info = ProductBll.GetModelByIpStatus("170.1.2.205",
                ProductAction.Testing);
            Assert.AreEqual("8CAB8EFA2730", info.ESN);
        }

        [Test]
        public void GetModelById_Get_ReturnsOne()
        {
            ProductInfo info = ProductBll.GetModelByRfid("123");
            Assert.NotNull(info);
        }

        [Test]
        public void GetModelByCraftStatus_Get_ReturnsOne()
        {
            ProductInfo info = ProductBll.GetModelByCraftStatus("2", ProductAction.EndTest);
            Assert.NotNull(info);
        }

        [Test]
        public void UpdateModel_Success_ChangeValue()
        {
            ProductInfo product = ProductBll.GetModelByIpStatus("170.1.2.205",
                ProductAction.OnLine);
            product.IsPass = ProductStatus.Fail.ToString();
            product.ActionName = ProductAction.EndTest.ToString();

            bool result = ProductBll.SureToUpdateModel(product);
            Assert.True(result);
        }

        [Test]
        public void GetModelByCraftStatus_Success_ReturnsModel()
        {
            ProductInfo product = ProductBll.GetModelByCraftStatus("Craft00005",
                ProductAction.Testing);
            Assert.AreEqual("8CAB8EFA2730", product.ESN);
        }
    }
}