using Newtonsoft.Json;

namespace AIS.Model.Rest.SignRequest
{
    public class ScAddRevocationInformation
    {
        [JsonProperty("@Type")]
        public string Type { get; set; }
    }
}
