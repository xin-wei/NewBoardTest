using BoardAutoTesting.Model;
using NUnit.Framework;

namespace BoardAutoTesting.Test
{
    [TestFixture]
    public class ClientConnectionTest
    {
        private ClientConnection ClientFactory()
        {
            return new ClientConnection();
        }

        [Test]
        public void Analyse_Success_ReturnsTrueCommand()
        {
            ClientConnection client = ClientFactory();
            string msg;
            bool result = client.Analyse("hehe*OUT#", out msg);
            Assert.AreEqual("*OUT#", msg);
            Assert.True(result);
        }

        [Test]
        public void GetSearchId_Success_ReturnsId()
        {
            ClientConnection client = ClientFactory();
            string rfid;
            bool result = client.GetSearchId("*12345678:IN?#", ":IN?", out rfid);
            Assert.True(result);
            Assert.AreEqual("12345678", rfid);
        }

    }
}