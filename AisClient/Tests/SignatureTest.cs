using System;
using System.Collections.Generic;
using System.IO;
using AIS;
using AIS.Model;
using AIS.Model.Rest;
using AIS.Rest;
using Common.Logging;
using Common.Logging.Simple;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Tests
{
    [TestClass]
    public class SignatureTest
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
        public void TestTimestampSignature()
        {
            var properties = ReadConfigurationFile("timestamp_config.json");
            Sign(properties, new SignatureMode(SignatureMode.Timestamp));
        }

        /**
        * Test with a Static signature that shows how to access all the configuration available and load it from a properties file. 
        */
        [TestMethod]
        public void TestTStaticSignature()
        {
            var properties = ReadConfigurationFile("static_config.json");
            Sign(properties, new SignatureMode(SignatureMode.Static));
        }

        /**
        * Test with an On Demand signature that shows how to access all the configuration available and load it from a properties file. 
        */
        [TestMethod]
        public void TestOnDemandSignature()
        {
            var properties = ReadConfigurationFile("on_demand_config.json");
            Sign(properties, new SignatureMode(SignatureMode.OnDemand));
        }

        /**
        * Test with an On Demand signature with Step Up that shows how to access all the configuration available and load it from a properties file. 
        */
        [TestMethod]
        public void TestOnDemandSignatureWithStepUp()
        {
            var properties = ReadConfigurationFile("on_demand_with_step_up_config.json");
            Sign(properties, new SignatureMode(SignatureMode.OnDemandStepUp));
        }

        /**
        * Test that shows how to configure the REST and AIS clients from the code.*/
        [TestMethod]
        public void TestFullyProgramaticConfiguration()
        {
            ConfigurationProperties properties = new ConfigurationProperties
            {
                ClientPollRounds = "10",
                ClientPollIntervalInSeconds = "10",
                ITextLicenseFilePath = "your-license-file",
                ServerRestSignUrl = "https://ais.swisscom.com/AIS-Server/rs/v1.0/sign",
                ServerRestPendingUrl = "https://ais.swisscom.com/AIS-Server/rs/v1.0/pending",
                ClientAuthKeyFile = "d:/Work/Swisscom/my-ais.key",
                ClientAuthKeyPassword = "93!Csupa!#!Kabra!",
                ClientCertFile = "d:/Work/Swisscom/my-ais.crt",
                SkipServerCertificateValidation = true,
                ClientHttpMaxConnectionsPerServer = "10",
                ClientHttpRequestTimeoutInSeconds = "10"
            };

            RestClientConfiguration restClientConfiguration = new RestClientConfiguration(properties);
            IRestClient restClient = new RestClient(restClientConfiguration);
            AisClientConfiguration aisClientConfiguration = new AisClientConfiguration(properties);
            try
            {
                IAisClient aisClient = new AisClient(restClient, aisClientConfiguration);
                UserData userData = new UserData
                {
                    TransactionId = Guid.NewGuid().ToString(),
                    ClaimedIdentityName = "ais-90days-trial",
                    ClaimedIdentityKey = "OnDemand-Advanced",
                    DistinguishedName = "cn=Test Gellert Abraham, givenname=Gellert, surname=Abraham, c=RO, serialnumber=0b5e3f1eb4b1a84b31ea3ff45fcab1049c95a00c",
                    StepUpLanguage = "en",
                    StepUpMessage = "Please confirm the signing of the document2",
                    StepUpMsisdn = "40740634075",
                    SignatureReason = "For testing purposes",
                    SignatureLocation = "Topeka, Kansas",
                    SignatureContactInfo = "test@test.com",
                    SignatureStandard = new SignatureStandard("PAdES-baseline"),
                    RevocationInformation = new RevocationInformation("PAdES-baseline"),
                    ConsentUrlCallback = new ConsentUrlCallback()
                };
                userData.ConsentUrlCallback.OnConsentUrlReceived += LogAtConsole;
                List<PdfHandle> documents = new List<PdfHandle>
                {
                    new PdfHandle
                    {
                        InputFileName = "in.pdf",
                        OutputFileName = "out.pdf",
                        DigestAlgorithm = DigestAlgorithm.SHA256
                    }
                };
                SignatureResult signatureResult = aisClient.SignWithOnDemandCertificateAndStepUp(documents, userData);
                Console.WriteLine($"Finished signing the document(s) with the status: {signatureResult}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception while signing the documents: {e}");
            }
        }

        public static void Sign(ConfigurationProperties properties, SignatureMode signatureMode)
        {
            AisClientConfiguration aisClientConfiguration = new AisClientConfiguration(properties);
            RestClientConfiguration restClientConfiguration = new RestClientConfiguration(properties);
            IRestClient restClient = new RestClient(restClientConfiguration);
            try
            {
                AisClient aisClient = new AisClient(restClient, aisClientConfiguration);
                SigningService signingService = new SigningService(aisClient);
                UserData userData = new UserData(properties);
                userData.ConsentUrlCallback.OnConsentUrlReceived += LogAtConsole;
                List<PdfHandle> documents = new List<PdfHandle>
                {
                    new PdfHandle
                    {
                        InputFileName = properties.LocalTestInputFile,
                        OutputFileName = properties.LocalTestOutputFile
                    }
                };
                SignatureResult signatureResult = signingService.Sign(documents, signatureMode, userData);
                Console.WriteLine($"Finished signing the document(s) with the status: {signatureResult}");
            }
            catch(Exception e)
            {
                Console.WriteLine($"Exception while signing the documents: {e}");
            }
        }

        private static void LogAtConsole(object sender, OnConsentUrlReceivedArgs e)
        {
            Console.WriteLine("Consent URL: " + e.Url);
        }

        private static ConfigurationProperties ReadConfigurationFile(string fileName)
        {
            return JsonConvert.DeserializeObject<ConfigurationProperties>(File.ReadAllText(fileName));
        }
    }
}
