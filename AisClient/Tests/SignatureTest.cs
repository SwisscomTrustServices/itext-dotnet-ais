/*
 * Copyright 2021 Swisscom Trust Services (Schweiz) AG
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
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
