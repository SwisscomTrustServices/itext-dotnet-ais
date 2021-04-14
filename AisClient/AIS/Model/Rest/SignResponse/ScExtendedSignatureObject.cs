using Newtonsoft.Json;

namespace AIS.Model.Rest.SignResponse
{
    public class ScExtendedSignatureObject
    {
        [JsonProperty("@WhichDocument")]
        public string WhichDocument { get; set; }

        [JsonProperty("Base64Signature")]
        public Base64Signature__1 Base64Signature { get; set; }

        [JsonProperty("Timestamp")]
        public Timestamp__1 Timestamp { get; set; }
    }
}
