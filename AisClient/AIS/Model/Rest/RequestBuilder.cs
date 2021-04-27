using System;
using System.Collections.Generic;
using System.Linq;
using AIS.Model.Rest.PendingRequest;
using AIS.Model.Rest.SignRequest;
using AIS.Utils;
using ClaimedIdentity = AIS.Model.Rest.SignRequest.ClaimedIdentity;
using OptionalInputs = AIS.Model.Rest.SignRequest.OptionalInputs;

namespace AIS.Model.Rest
{
    public class RequestBuilder
    {
        private static readonly string SwisscomBasicProfile = "http://ais.swisscom.ch/1.1";

        public AISSignRequest BuildAisSignRequest(SignRequestDetails requestDetails)
        {
            // Input documents --------------------------------------------------------------------------------
            List<DocumentHash> documentHashes = requestDetails.Documents.Select(d => new DocumentHash
            {
                Id = d.Id, 
                DsigDigestMethod = new DsigDigestMethod {Algorithm = d.DigestAlgorithm.Uri},
                DsigDigestValue = d.Base64HashToSign
            }).ToList();
            InputDocuments inputDocuments = new InputDocuments{DocumentHash = documentHashes};

            // Optional inputs --------------------------------------------------------------------------------
            OptionalInputs optionalInputs = new OptionalInputs
            {
                SignatureType = requestDetails.SignatureType.Uri,
                AdditionalProfile = requestDetails.AdditionalProfiles.Select(ap => ap.Uri).ToList()
            };

            if (requestDetails.SignatureMode.FriendlyName != SignatureMode.Timestamp &&
                requestDetails.UserData.SignatureStandard.Value != SignatureStandard.Default)
            {
                optionalInputs.ScSignatureStandard = requestDetails.UserData.SignatureStandard.Value;
            }

            optionalInputs.AddTimestamp = requestDetails.UserData.IsAddTimestamp()
                ? new AddTimestamp {Type = SignatureType.Timestamp.Uri}
                : null;
            
            ClaimedIdentity claimedIdentity = new ClaimedIdentity { Name = requestDetails.UserData.ClaimedIdentityName };
            if (requestDetails.SignatureMode.FriendlyName != SignatureMode.Timestamp && !requestDetails.UserData.ClaimedIdentityKey.IsEmpty())
            {
                claimedIdentity.Name += ":" + requestDetails.UserData.ClaimedIdentityKey;
            }
            optionalInputs.ClaimedIdentity = claimedIdentity;

            ScCertificateRequest certificateRequest = null;
            if (requestDetails.WithCertificateRequest)
            {
                certificateRequest = new ScCertificateRequest {ScDistinguishedName = requestDetails.UserData.DistinguishedName};
            }

            if (requestDetails.WithStepUp)
            {
                if (certificateRequest == null)
                {
                    certificateRequest = new ScCertificateRequest();
                }
                certificateRequest.ScStepUpAuthorisation = new ScStepUpAuthorisation{ScPhone = 
                    new ScPhone
                    {
                        ScLanguage = requestDetails.UserData.StepUpLanguage,
                        ScMSISDN = requestDetails.UserData.StepUpMsisdn,
                        ScMessage = requestDetails.UserData.StepUpMessage,
                        ScSerialNumber = requestDetails.UserData.StepUpSerialNumber
                    }};
            }

            optionalInputs.ScCertificateRequest = certificateRequest;

            ScAddRevocationInformation addRevocationInformation = new ScAddRevocationInformation();
            if (requestDetails.UserData.RevocationInformation.Value == RevocationInformation.Default)
            {
                if (requestDetails.SignatureMode.FriendlyName == SignatureMode.Timestamp)
                {
                    addRevocationInformation.Type = RevocationInformation.Both;
                }
                else
                {
                    addRevocationInformation.Type = null;
                }
            }
            else
            {
                addRevocationInformation.Type = requestDetails.UserData.RevocationInformation.Value;
            }

            optionalInputs.ScAddRevocationInformation = addRevocationInformation;

            return new AISSignRequest{ SignRequest = new SignRequest.SignRequest
            {
                InputDocuments = inputDocuments,
                OptionalInputs = optionalInputs,
                RequestId = GenerateRequestId(),
                Profile = SwisscomBasicProfile
            }
            };
        }

        public static AISPendingRequest BuildAisPendingRequest(string asyncResponseId, UserData userData)
        {
            return new AISPendingRequest
            {
                AsyncPendingRequest = new AsyncPendingRequest
                {
                    OptionalInputs = new PendingRequest.OptionalInputs
                    {
                        ClaimedIdentity = new PendingRequest.ClaimedIdentity
                        {
                            Name = userData.ClaimedIdentityName
                        },
                        AsyncResponseId = asyncResponseId
                    },
                    Profile = SwisscomBasicProfile
                }
            };
        }

        private static string GenerateRequestId()
        {
            return "ID-" + Guid.NewGuid();
        }

    }
}
