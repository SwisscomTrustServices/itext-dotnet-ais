using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AIS.Model;
using AIS.Model.Rest;
using AIS.Model.Rest.SignRequest;
using AIS.Model.Rest.SignResponse;
using AIS.Rest;
using AIS.Utils;

namespace AIS
{
    public class AisClient : IAisClient
    {
        private readonly RestClient restClient;
        private AisClientConfiguration aisClientConfiguration;

        public AisClient(RestClient restClient, AisClientConfiguration aisClientConfiguration)
        {
            this.restClient = restClient;
            this.aisClientConfiguration = aisClientConfiguration;
        }

        public AisClient(RestClient restClient)
        {
            this.restClient = restClient;
            aisClientConfiguration = new AisClientConfiguration{SignaturePollingIntervalInSeconds = 10, SignaturePollingRounds = 10};
        }

        public SignatureResult SignWithStaticCertificate(List<PdfHandle> documentHandles, UserData userData)
        {
            return PerformSigning(new SignRequestDetails
            {
                AdditionalProfiles = new List<AdditionalProfile>(),
                DocumentHandles = documentHandles,
                SignatureMode = new SignatureMode(SignatureMode.Static),
                SignatureType = SignatureType.Cms,
                UserData = userData,
                WithStepUp = false,
                WithCertificateRequest = false
            });
        }

        public SignatureResult SignWithOnDemandCertificate(List<PdfHandle> documentHandles, UserData userData)
        {
            return PerformSigning(new SignRequestDetails
            {
                AdditionalProfiles = new List<AdditionalProfile> { AdditionalProfile.OnDemandCertificate },
                DocumentHandles = documentHandles,
                SignatureMode = new SignatureMode(SignatureMode.OnDemand),
                SignatureType = SignatureType.Timestamp,
                UserData = userData,
                WithStepUp = false,
                WithCertificateRequest = true
            });
        }

        public SignatureResult SignWithOnDemandCertificateAndStepUp(List<PdfHandle> documentHandles, UserData userData)
        {
            return PerformSigning(new SignRequestDetails
            {
                AdditionalProfiles = new List<AdditionalProfile> { AdditionalProfile.OnDemandCertificate, AdditionalProfile.Redirect, AdditionalProfile.Async },
                DocumentHandles = documentHandles,
                SignatureMode = new SignatureMode(SignatureMode.OnDemandStepUp),
                SignatureType = SignatureType.Cms,
                UserData = userData,
                WithStepUp = true,
                WithCertificateRequest = true
            });
        }

        public SignatureResult Timestamp(List<PdfHandle> documentHandles, UserData userData)
        {
            return PerformSigning(new SignRequestDetails
            {
                AdditionalProfiles = new List<AdditionalProfile> {AdditionalProfile.Timestamp},
                DocumentHandles = documentHandles,
                SignatureMode = new SignatureMode(SignatureMode.Timestamp),
                SignatureType = SignatureType.Timestamp,
                UserData = userData,
                WithStepUp = false,
                WithCertificateRequest = false
            });
        }

        private SignatureResult PerformSigning(SignRequestDetails details)
        {
            Trace trace = new Trace(details.UserData.TransactionId);
            details.UserData.ValidateYourself(details.SignatureMode, trace);
            details.DocumentHandles.ForEach(dh => dh.ValidateYourself(trace));
            List<PdfDocumentHandler> documentsToSign = details.DocumentHandles.Select(dh =>
                PrepareDocumentForSigning(dh, details.SignatureMode, SignatureType.Timestamp, details.UserData, trace)).ToList();
            try
            {
                List<AdditionalProfile> additionalProfiles = PrepareAdditionalProfiles(documentsToSign, details.AdditionalProfiles);

                AISSignRequest signRequest = RequestBuilder.BuildAisSignRequest(new SignRequestDetails
                {
                    AdditionalProfiles = additionalProfiles,
                    Documents = documentsToSign,
                    SignatureMode = details.SignatureMode,
                    SignatureType = details.SignatureType,
                    UserData = details.UserData,
                    WithStepUp = details.WithStepUp,
                    WithCertificateRequest = details.WithCertificateRequest
                });
                AISSignResponse signResponse = restClient.RequestSignature(signRequest, trace);
                if (details.WithStepUp && !ResponseUtils.IsResponseAsyncPending(signResponse))
                {
                    return ExtractSignatureResultFromResponse(signResponse, trace);
                }
                if (details.WithStepUp)
                {
                    signResponse = PollUntilSignatureIsComplete(signResponse, details.UserData, trace);
                }
                if (!ResponseUtils.IsResponseMajorSuccess(signResponse))
                {
                    return ExtractSignatureResultFromResponse(signResponse, trace);
                }
                FinishDocumentSigning(documentsToSign, signResponse, details.SignatureMode, details.SignatureType.EstimatedSignatureSizeInBytes, trace);
                return SignatureResult.Success;
            }
            catch (Exception e)
            {
                throw new Exception(
                    $"Failed to communicate with the AIS service and obtain the signature(s) - {trace.Id}", e);
            }
            finally
            {
                documentsToSign.ForEach(d => d.Close());
            }
        }

        private AISSignResponse PollUntilSignatureIsComplete(AISSignResponse signResponse, UserData detailsUserData, Trace trace)
        {
            throw new NotImplementedException();
        }

        private SignatureResult ExtractSignatureResultFromResponse(AISSignResponse response, Trace trace)
        {
            if (response?.SignResponse?.Result?.ResultMajor == null)
            {
                throw new Exception($"Incomplete response received from the AIS service: {response} - {trace.Id}");
            }

            ResultMajorCode resultMajorCode = ResultMajorCode.GetByUri(response.SignResponse.Result.ResultMajor);
            ResultMinorCode resultMinorCode = ResultMinorCode.GetByUri(response.SignResponse.Result.ResultMinor);
            if (resultMajorCode == null)
            {
                throw new Exception($"Failure response received from the AIS service: {ResponseUtils.GetResponseResultSummary(response)} - {trace.Id}");
            }

            if (resultMajorCode.Uri == ResultMajorCode.Success.Uri)
            {
                return SignatureResult.Success;
            }
            if (resultMajorCode.Uri == ResultMajorCode.Pending.Uri)
            {
                return SignatureResult.UserTimeout;
            }
            if (resultMajorCode.Uri == ResultMajorCode.RequesterError.Uri || resultMajorCode.Uri == ResultMajorCode.SubsystemError.Uri)
            {
                SignatureResult? result = ExtractSignatureResultFromMinorCode(resultMinorCode, response.SignResponse.Result);
                if (result != null)
                {
                    return result.Value;
                }
            }
            throw new Exception($"Failure response received from the AIS service: {ResponseUtils.GetResponseResultSummary(response)} - {trace.Id}");
        }

        private SignatureResult? ExtractSignatureResultFromMinorCode(object minorCode, object responseResult)
        {
            throw new NotImplementedException();
        }


        private List<AdditionalProfile> PrepareAdditionalProfiles(List<PdfDocumentHandler> documentsToSign, List<AdditionalProfile> defaultProfiles)
        {
            List<AdditionalProfile> additionalProfiles = new List<AdditionalProfile>();
            if (documentsToSign.Count > 1)
            {
                additionalProfiles.Add(AdditionalProfile.Batch);
            }
            additionalProfiles.AddRange(defaultProfiles);
            return additionalProfiles;
        }

        private PdfDocumentHandler PrepareDocumentForSigning(PdfHandle documentHandle, SignatureMode signatureMode,
            SignatureType signatureType, UserData userData, Trace trace)
        {
            //info
            Console.WriteLine($"Preparing {signatureMode.FriendlyName} signing for document: {documentHandle.InputFileName} - {trace.Id}");
            var inputStream = File.ReadAllBytes(documentHandle.InputFileName);
            PdfDocumentHandler newDocument = new PdfDocumentHandler(inputStream, trace);
            newDocument.PrepareForSigning(documentHandle.DigestAlgorithm, signatureType, userData);
            return newDocument;
        }

        private void FinishDocumentSigning(List<PdfDocumentHandler> documents, AISSignResponse response,
            SignatureMode signatureMode, int signatureEstimatedSize, Trace trace)
        {
            List<string> encodedCrlEntries = ResponseUtils.ExtractScCRLs(response);
            List<string> encodedOcspEntries = ResponseUtils.ExtractScOCSPs(response);
            bool containsSingleDocument = documents.Count == 1;
            //TODO log
            documents.ForEach(d =>
            {
                string encodedSignature = ExtractEncodedSignature(response, containsSingleDocument, signatureMode, d);
                var content = d.CreateSignedPdf(Convert.FromBase64String(encodedSignature), signatureEstimatedSize, encodedCrlEntries, encodedOcspEntries);
                File.WriteAllBytes("a.pdf", content);
            });
        }

        private string ExtractEncodedSignature(AISSignResponse response, bool containsSingleDocument, SignatureMode signatureMode, PdfDocumentHandler pdfDocumentHandler)
        {
            SignatureObject signatureObject = response.SignResponse.SignatureObject;
            return signatureMode.FriendlyName == SignatureMode.Timestamp
                ? signatureObject.Timestamp.RFC3161TimeStampToken
                : signatureObject.Base64Signature.S;
        }
    }
}
