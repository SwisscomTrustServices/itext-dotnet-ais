using Newtonsoft.Json;

namespace AIS.Model.Rest.SignRequest
{
    public class AddTimestamp
    {
        [JsonProperty("@Type")]
        public string Type { get; set; }
    }
}
