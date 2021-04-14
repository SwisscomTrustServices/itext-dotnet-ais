using System;
using System.Collections.Generic;
using AIS.Model.Rest;
using AIS.Model.Rest.SignResponse;

namespace AIS.Utils
{
    public class ResponseUtils
    {
        public static List<string> ExtractScCRLs(AISSignResponse response)
        {
            var result = new List<string>();
            if (response?.SignResponse?.OptionalOutputs?.ScRevocationInformation?.ScCRLs?.ScCRL != null)
            {
                result.AddRange(response.SignResponse.OptionalOutputs.ScRevocationInformation.ScCRLs.ScCRL);
            }
            return result;
        }

        public static List<string> ExtractScOCSPs(AISSignResponse response)
        {
            var result = new List<string>();
            if (response?.SignResponse?.OptionalOutputs?.ScRevocationInformation?.ScOCSPs?.ScOCSP != null)
            {
                result.AddRange(response.SignResponse.OptionalOutputs.ScRevocationInformation.ScOCSPs.ScOCSP);
            }
            return result;
        }

        public static bool IsResponseAsyncPending(AISSignResponse response)
        {
            return response?.SignResponse?.Result?.ResultMajor == ResultMajorCode.Pending.Uri;
        }

        public static bool IsResponseMajorSuccess(AISSignResponse response)
        {
            return response?.SignResponse?.Result?.ResultMajor == ResultMajorCode.Success.Uri;
        }

        public static string GetResponseResultSummary(AISSignResponse response)
        {
            Result result = response.SignResponse.Result;
            return "Major=[" + result.ResultMajor + "], "
                   + "Minor=[" + result.ResultMinor + "], "
                   + "Message=[" + result.ResultMessage + ']';
        }

        public static bool ResponseHasStepUpConsentUrl(AISSignResponse response)
        {
            return response?.SignResponse?.OptionalOutputs?.ScStepUpAuthorisationInfo?.ScResult.ScConsentURL != null;
        }

        public static string GetStepUpConsentUrl(AISSignResponse response)
        {
            return response.SignResponse.OptionalOutputs.ScStepUpAuthorisationInfo.ScResult.ScConsentURL;
        }

        public static string getResponseId(AISSignResponse response)
        {
            return response.SignResponse.OptionalOutputs.AsyncResponseID;
        }

        public static ScExtendedSignatureObject GetSignatureObjectByDocumentId(string documentId, AISSignResponse signResponse)
        {
            ScSignatureObjects signatureObjects = signResponse.SignResponse.SignatureObject.Other.ScSignatureObjects;
            foreach (var seSignatureObject in signatureObjects.ScExtendedSignatureObject)
            {
                if (documentId == seSignatureObject.WhichDocument)
                {
                    return seSignatureObject;
                }
            }
            throw new Exception($"Invalid AIS response. Cannot find the extended signature object for document with ID=[{documentId}]");
        }
    }
}
