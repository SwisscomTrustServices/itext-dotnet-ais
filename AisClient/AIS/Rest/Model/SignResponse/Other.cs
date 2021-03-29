using Newtonsoft.Json;

namespace AIS.Rest.Model.SignResponse
{
    public class Other
    {
        [JsonProperty("sc.SignatureObjects")]
        public ScSignatureObjects ScSignatureObjects { get; set; }
    }
}