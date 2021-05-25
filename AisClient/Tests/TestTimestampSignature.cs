using AIS.Model;
using Common.Logging;
using Common.Logging.Simple;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class TestTimestampSignature
    {
        [TestInitialize]
        public void Initialize()
        {
            LogManager.Adapter = new ConsoleOutLoggerFactoryAdapter { Level = LogLevel.Debug };
        }

        /**
        * Test with a Timestamp signature that shows how to access all the configuration available and load it from a properties file.
        */
        [TestMethod]
        public void TestSignature()
        {
            var properties = SignatureTest.ReadConfigurationFile("timestamp_config.json");
            SignatureTest.Sign(properties, new SignatureMode(SignatureMode.Timestamp));
        }
    }
}
