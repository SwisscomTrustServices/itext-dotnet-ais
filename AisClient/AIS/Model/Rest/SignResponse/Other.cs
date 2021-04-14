using Newtonsoft.Json;

namespace AIS.Model.Rest.SignResponse
{
    public class Other
    {
        [JsonProperty("sc.SignatureObjects")]
        public ScSignatureObjects ScSignatureObjects { get; set; }
    }
}