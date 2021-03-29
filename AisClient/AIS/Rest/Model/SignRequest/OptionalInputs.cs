using System.Collections.Generic;
using Newtonsoft.Json;

namespace AIS.Rest.Model.SignRequest
{
    public class OptionalInputs
    {
        public AddTimestamp AddTimestamp { get; set; }

        public List<string> AdditionalProfile { get; set; }

        public ClaimedIdentity ClaimedIdentity { get; set; }

        public string SignatureType { get; set; }

        [JsonProperty("sc.AddRevocationInformation")]
        public ScAddRevocationInformation ScAddRevocationInformation { get; set; }

        [JsonProperty("sc.SignatureStandard")]
        public string ScSignatureStandard { get; set; }

        [JsonProperty("sc.CertificateRequest")]
        public ScCertificateRequest ScCertificateRequest { get; set; }
    }
}
