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
using AIS;
using AIS.Model;
using AIS.Model.Rest;
using AIS.Rest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class TestFullyProgramaticConfiguration
    {
        /**
        * Test that shows how to configure the REST and AIS clients from the code.
        */
        [TestMethod]
        public void TestSignature()
        {
            ConfigurationProperties properties = new ConfigurationProperties
            {
                ClientPollRounds = "10",
                ClientPollIntervalInSeconds = "10",
                ITextLicenseFilePath = "your-license-file",
                ServerRestSignUrl = "https://ais.swisscom.com/AIS-Server/rs/v1.0/sign",
                ServerRestPendingUrl = "https://ais.swisscom.com/AIS-Server/rs/v1.0/pending",
                ClientAuthKeyFile = "d:/Work/Swisscom/my-ais.key",
                ClientAuthKeyPassword = "your-password",
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
                    StepUpMsisdn = "40740634075123",
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
                        InputFileName = "input.pdf",
                        OutputFileName = "output-programatic.pdf",
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

        private static void LogAtConsole(object sender, OnConsentUrlReceivedArgs e)
        {
            Console.WriteLine("Consent URL: " + e.Url);
        }
    }
}
