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
        public void GetProductInfoByIpStatus_Get_ReturnsModel()
        {
            ProductInfo info  = ProductBll.GetProductInfoByIpStatus(".18", ProductAction.Testing);
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
            ProductInfo info = ProductBll.GetModelByCraftStatus("2", ProductAction.EndTest);
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

            bool result = ProductBll.SureToUpdateModel(info);
            Assert.True(result);
        }
    }
}