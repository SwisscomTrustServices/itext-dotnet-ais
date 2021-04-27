using System;
using System.Collections.Generic;
using System.Text;
using AIS.Model;

namespace AIS
{
    public class SigningService
    {
        private AisClient client;

        public SigningService(AisClient aisClient)
        {
            client = aisClient;
        }

        public SignatureResult Sign(List<PdfHandle> documents, SignatureMode signatureMode, UserData userData)
        {
            switch (signatureMode.FriendlyName)
            {
                case SignatureMode.Static:
                    return client.SignWithStaticCertificate(documents, userData);
                case SignatureMode.OnDemand:
                    return client.SignWithOnDemandCertificate(documents, userData);
                case SignatureMode.OnDemandStepUp:
                    return client.SignWithOnDemandCertificateAndStepUp(documents, userData);
                case SignatureMode.Timestamp:
                    return client.Timestamp(documents, userData);
                default:
                    throw new ArgumentException($"Invalid signature mode. Can not sign the document(s) with the {signatureMode} signature. - {userData.TransactionId}");
            }
        }
    }
}
