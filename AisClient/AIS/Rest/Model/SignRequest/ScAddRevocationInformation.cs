using Newtonsoft.Json;

namespace AIS.Rest.Model.SignRequest
{
    public class ScAddRevocationInformation
    {
        [JsonProperty("@Type")]
        public string Type { get; set; }
    }
}
