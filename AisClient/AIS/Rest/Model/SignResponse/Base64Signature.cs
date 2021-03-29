using Newtonsoft.Json;

namespace AIS.Rest.Model.SignResponse
{
    public class Base64Signature
    {
        [JsonProperty("$")]
        public string S { get; set; }

        [JsonProperty("@Type")]
        public string Type { get; set; }
    }
}
