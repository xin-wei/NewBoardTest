using BoardAutoTesting.BLL;
using BoardAutoTesting.Status;
using Model;
using NUnit.Framework;

namespace BoardAutoTesting.Test
{
    [TestFixture]
    public class ProductBllTest
    {
        [Test]
        public void GetProductInfoByIpStatus_Get_ReturnsModel()
        {
            ProductInfo info  = ProductBll.GetProductInfoByIpStatus(".18", ProductStatus.Testing);
            Assert.NotNull(info);
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
            ProductInfo info = ProductBll.GetModelByCraftStatus("2", ProductStatus.OnCraft);
            Assert.NotNull(info);
        }

        [Test]
        public void UpdateModel_Success_ChangeValue()
        {
            ProductInfo info = new ProductInfo
            {
                RFID = "123",
                ESN = "123",
                IsPass = "Fail",
                RouteName = "2.4",
                CraftID = "1",
                CurrentIp = ".18",
                OldIp = "",
                ActionName = "3",
                ATEIp = ".11"
            };

            int i = ProductBll.UpdateModel(info);
            Assert.AreEqual(1, i);
        }
    }
}