using AIS.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class TestStaticSignature
    {
        /**
        * Test with a Static signature that shows how to access all the configuration available and load it from a properties file. 
        */
        [TestMethod]
        public void TestSignature()
        {
            var properties = SignatureTest.ReadConfigurationFile("static_config.json");
            SignatureTest.Sign(properties, new SignatureMode(SignatureMode.Static));
        }
    }
}
