using System;
using System.Collections.Generic;
using System.IO;
using AIS;
using AIS.Model;
using AIS.Rest;
using Common.Logging;
using Common.Logging.Simple;
using Newtonsoft.Json;

namespace Tests
{
    public static class SignatureTest
    {

        public static void Sign(ConfigurationProperties properties, SignatureMode signatureMode)
        {
            LogManager.Adapter = new ConsoleOutLoggerFactoryAdapter { Level = LogLevel.Debug };
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

        public static ConfigurationProperties ReadConfigurationFile(string fileName)
        {
            return JsonConvert.DeserializeObject<ConfigurationProperties>(File.ReadAllText(fileName));
        }

        private static void LogAtConsole(object sender, OnConsentUrlReceivedArgs e)
        {
            Console.WriteLine("Consent URL: " + e.Url);
        }
    }
}
