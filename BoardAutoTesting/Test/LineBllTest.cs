using BoardAutoTesting.BLL;
using BoardAutoTesting.Model;
using NUnit.Framework;

namespace BoardAutoTesting.Test
{
    [TestFixture]
    public class LineBllTest
    {
        [Test]
        public void GetModelByRouteEmptyCraft_Success_ReturnsOne()
        {
            LineInfo info = LineBll.GetModelByRouteEmptyCraft("5.8");
            Assert.AreEqual("216", info.CraftId);
        }

        [Test]
        public void UpdateModel_Success_ValueChange()
        {
            LineInfo line = new LineInfo
            {
                CraftId = "213",
                RouteName = "2.4",
                LineIdx = "oe",
                McuIp = ".15",
                AteIp = ".16",
                IsRepair = false,
                IsOut = false,
                LineEsn = "1",
                CraftEsn = "d",
                PortId = "NA",
            };
            bool result = LineBll.SureToUpdateModel(line, "Mcu_Ip");
            Assert.True(result);
        }

        [Test]
        public void GetModelByIpPort_Success_ReturnsOne()
        {
            LineInfo line = LineBll.GetModelByIpPort(".15", "NA");
            Assert.AreEqual("d", line.CraftEsn);
        }

        [Test] public void GetModelByIpPort_Empty_ReturnsNull()
        {
            LineInfo line = LineBll.GetModelByIpPort("", "NA");
            Assert.IsNull(line);
        }

    }
}