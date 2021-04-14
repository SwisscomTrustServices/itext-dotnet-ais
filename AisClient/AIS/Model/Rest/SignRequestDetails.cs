using System.Collections.Generic;

namespace AIS.Model.Rest
{
    public class SignRequestDetails
    {
        public List<PdfDocumentHandler> Documents { get; set; }

        public List<PdfHandle> DocumentHandles { get; set; }

        public SignatureMode SignatureMode { get; set; }

        public SignatureType SignatureType { get; set; }

        public UserData UserData { get; set; }

        public List<AdditionalProfile> AdditionalProfiles { get; set; }

        public bool WithStepUp { get; set; }

        public bool WithCertificateRequest { get; set; }
    }
}
