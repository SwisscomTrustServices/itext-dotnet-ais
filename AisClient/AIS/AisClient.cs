using System;
using System.Collections.Generic;
using AIS.Model;

namespace AIS
{
    public class AisClient : IAisClient
    {
        public SignatureResult SignWithStaticCertificate(List<PdfHandle> documentHandles, UserData userData)
        {
            throw new NotImplementedException();
        }

        public SignatureResult SignWithOnDemandCertificate(List<PdfHandle> documentHandles, UserData userData)
        {
            throw new NotImplementedException();
        }

        public SignatureResult SignWithOnDemandCertificateAndStepUp(List<PdfHandle> documentHandles, UserData userData)
        {
            throw new NotImplementedException();
        }

        public SignatureResult Timestamp(List<PdfHandle> documentHandles, UserData userData)
        {
            throw new NotImplementedException();
        }
    }
}
