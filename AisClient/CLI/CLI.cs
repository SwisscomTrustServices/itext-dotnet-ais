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

namespace CLI
{
    class CLI
    {
        private const string Separator = "--------------------------------------------------------------------------------";
        private const string TypeStatic = "static";
        private const string TypeOnDemand = "ondemand";
        private const string TypeOnDemandStepUp = "ondemand-stepup";
        private const string TypeTimestamp = "timestamp";

        //private static ClientVersionProvider versionProvider;


        private static ILog logger;

        static void Main(string[] args)
        {
            var arguments = new ArgumentsService().GetArguments(args);
            if (!arguments.Complete)
            {
                return;
            }

            ConfigureLogging(arguments.LogLevel);
            LogStartingParameters(arguments);
            SignDocuments(arguments);
            //TODO versioning
        }

        private static void ConfigureLogging(LogLevel logLevel)
        {
            LogManager.Adapter = new ConsoleOutLoggerFactoryAdapter { Level = logLevel };
            logger = LogManager.GetLogger<CLI>();
        }

        private static void LogStartingParameters(ArgumentsDto arguments)
        {
            logger.Info(Separator);
            logger.Info("Starting with following parameters:");
            logger.Info("Config            : " + arguments.ConfigFile);
            logger.Info("Input file(s)     : " + string.Join(", ", arguments.InputFiles));
            logger.Info("Output file       : " + arguments.OutputFile);
            logger.Info("Suffix            : " + arguments.Suffix);
            logger.Info("Type of signature : " + arguments.SignatureType);
            logger.Info("Verbose level     : " + arguments.LogLevel);
            logger.Info(Separator);

        }

        private static void SignDocuments(ArgumentsDto arguments)
        {
            ConfigurationProperties configurationProperties = DeserializeConfig(arguments.ConfigFile);
            UserData userData = new UserData(configurationProperties);
            userData.ConsentUrlCallback.OnConsentUrlReceived += LogAtConsole;
            RestClientConfiguration restClientConfiguration = new RestClientConfiguration(configurationProperties);
            IRestClient restClient = new RestClient(restClientConfiguration);
            IAisClient aisClient = new AisClient(restClient, new AisClientConfiguration(configurationProperties));

            List<PdfHandle> documentsToSign = SetDocumentsToSign(arguments);

            logger.Info($"Start performing the signings for the input file(s). " +
                        $"You can trace the corresponding details using the {userData.TransactionId} trace id.");
            SignatureResult result;
            switch (arguments.SignatureType)
            {
                case TypeStatic:
                {
                    result = aisClient.SignWithStaticCertificate(documentsToSign, userData);
                    break;
                }
                case TypeOnDemand:
                {
                    result = aisClient.SignWithOnDemandCertificate(documentsToSign, userData);
                    break;
                }
                case TypeOnDemandStepUp:
                {
                    result = aisClient.SignWithOnDemandCertificateAndStepUp(documentsToSign, userData);
                    break;
                }
                case TypeTimestamp:
                {
                    result = aisClient.Timestamp(documentsToSign, userData);
                    break;
                }
                default:
                {
                    throw new ArgumentException("Invalid type: " + arguments.SignatureType);
                }
            }

            logger.Info($"Signature(s) final result: {result} - {userData.TransactionId}");
        }
        
        private static void LogAtConsole(object sender, OnConsentUrlReceivedArgs e)
        {
            Console.WriteLine("Consent URL: " + e.Url);
        }


        private static List<PdfHandle> SetDocumentsToSign(ArgumentsDto arguments)
        {
            List<PdfHandle> documentsToSign = new List<PdfHandle>();

            foreach (var inputFile in arguments.InputFiles)
            {
                PdfHandle document = new PdfHandle
                {
                    InputFileName = inputFile,
                    OutputFileName = arguments.OutputFile ?? GenerateOutputFileName(inputFile, arguments.Suffix)
                };
                documentsToSign.Add(document);
            }

            return documentsToSign;
        }

        private static string GenerateOutputFileName(string inputFileName, string suffix)
        {
            string finalSuffix = suffix.Replace("#time",
                TimeZone.CurrentTimeZone.ToLocalTime(DateTime.Now).ToString("yyyyMMdd-HHmmss"));

            int lastDotIndex = inputFileName.LastIndexOf('.');

            if (lastDotIndex < 0)
            {
                return inputFileName + finalSuffix;
            }

            return inputFileName.Substring(0, lastDotIndex) + finalSuffix + inputFileName.Substring(lastDotIndex);
        }

        private static ConfigurationProperties DeserializeConfig(string path)
        {
            string aisConfig = File.ReadAllText(path);

            return JsonConvert.DeserializeObject<ConfigurationProperties>(aisConfig);
        }
    }
}
