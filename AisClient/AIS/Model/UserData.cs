﻿/*
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
using System.IO;
using AIS.Common;
using AIS.Utils;
using Newtonsoft.Json;

namespace AIS.Model
{
    public class UserData
    {
        public string TransactionId { get; set; }

        public string ClaimedIdentityName { get; set; }
        public string ClaimedIdentityKey { get; set; }
        public string DistinguishedName { get; set; }

        public string StepUpLanguage { get; set; }
        public string StepUpMsisdn { get; set; }
        public string StepUpMessage { get; set; }
        public string StepUpSerialNumber { get; set; }

        public string SignatureName { get; set; }
        public string SignatureReason { get; set; }
        public string SignatureLocation { get; set; }
        public string SignatureContactInfo { get; set; }

        public ConsentUrlCallback ConsentUrlCallback { get; set; }

        private bool addTimestamp = true;
        public RevocationInformation RevocationInformation { get; set; }
        public SignatureStandard SignatureStandard { get; set; }

        public UserData()
        {
        }

        public UserData(ConfigurationProperties configuration)
        {
            SetTransactionIdToRandomUuid();
            SetFromConfiguration(configuration);
        }

        public UserData(ConfigurationProperties configuration, string transactionId)
        {
            TransactionId = transactionId;
            SetFromConfiguration(configuration);
        }

        public void SetFromConfigurationFile(string filePath)
        {
            string config = File.ReadAllText(filePath);
            SetFromConfiguration(JsonConvert.DeserializeObject<ConfigurationProperties>(config));
        }

        private void SetTransactionIdToRandomUuid()
        {
            TransactionId = Guid.NewGuid().ToString();
        }

        private void SetFromConfiguration(ConfigurationProperties configuration)
        {
            ClaimedIdentityName = ConfigParser.GetStringNotNull("SignatureClaimedIdentityName", configuration.SignatureClaimedIdentityName);
            ClaimedIdentityKey = configuration.SignatureClaimedIdentityKey;
            StepUpLanguage = configuration.SignatureStepUpLanguage;
            StepUpMsisdn = configuration.SignatureStepUpMsisdn;
            StepUpMessage = configuration.SignatureStepUpMessage;
            StepUpSerialNumber = configuration.SignatureStepUpSerialNumber;
            DistinguishedName = ConfigParser.GetStringNotNull("SignatureDistinguishedName", configuration.SignatureDistinguishedName);
            SignatureName = configuration.SignatureName;
            SignatureReason = configuration.SignatureReason;
            SignatureLocation = configuration.SignatureLocation;
            SignatureContactInfo = configuration.SignatureContactInfo;
            SignatureStandard = new SignatureStandard(configuration.SignatureStandard);
            RevocationInformation = new RevocationInformation(configuration.SignatureRevocationInformation);
            if (bool.TryParse(configuration.SignatureAddTimestamp, out var value))
            {
                addTimestamp = value;
            }
            ConsentUrlCallback = new ConsentUrlCallback();
        }

        public void ValidatePropertiesForSignature(SignatureMode signatureMode, Trace trace)
        {
            if (string.IsNullOrEmpty(TransactionId))
            {
                throw new AisClientException("The user data's transactionId cannot be null or empty. For example, you can set it to a new UUID "
                                             + "or to any other value that is unique between requests. This helps with traceability in the logs "
                                             + "generated by the AIS client");
            }

            Validator.AssertValueNotNull(ClaimedIdentityName, "Claimed identity must be provided", trace);

            switch (signatureMode.FriendlyName)
            {
                case SignatureMode.Static:
                    break;
                case SignatureMode.OnDemand:
                    break;
                case SignatureMode.OnDemandStepUp:
                    Validator.AssertValueNotNull(StepUpLanguage, "The step up language must be provided", trace);
                    Validator.AssertValueNotNull(StepUpMessage, "The step up message must be provided", trace);
                    Validator.AssertValueNotNull(StepUpMsisdn, "The step up msisdn must be provided", trace);
                    break;
                case SignatureMode.Timestamp:
                    break;
                default:
                    throw new AisClientException($"Invalid signature mode: {signatureMode.FriendlyName}");
            }
        }

        public bool IsAddTimestamp()
        {
            return addTimestamp;
        }
    }
}
