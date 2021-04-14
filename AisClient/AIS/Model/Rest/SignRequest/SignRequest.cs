using Newtonsoft.Json;

namespace AIS.Model.Rest.SignRequest
{
    public class SignRequest
    {
        [JsonProperty("@Profile")]
        public string Profile { get; set; }

        [JsonProperty("@RequestID")]
        public string RequestId { get; set; }

        public InputDocuments InputDocuments { get; set; }

        public OptionalInputs OptionalInputs { get; set; }
    }
}
