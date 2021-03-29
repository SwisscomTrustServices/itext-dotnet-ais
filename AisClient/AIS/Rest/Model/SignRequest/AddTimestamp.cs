using Newtonsoft.Json;

namespace AIS.Rest.Model.SignRequest
{
    public class AddTimestamp
    {
        [JsonProperty("@Type")]
        public string Type { get; set; }
    }
}
