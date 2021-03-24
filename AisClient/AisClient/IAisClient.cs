using System.Collections.Generic;
using AIS.Model;

namespace AIS
{
    public interface IAisClient
    {
        SignatureResult SignWithStaticCertificate(List<PdfHandle> documentHandles, UserData userData);

        SignatureResult SignWithOnDemandCertificate(List<PdfHandle> documentHandles, UserData userData);

        SignatureResult SignWithOnDemandCertificateAndStepUp(List<PdfHandle> documentHandles, UserData userData);

        SignatureResult Timestamp(List<PdfHandle> documentHandles, UserData userData);
    }
}
