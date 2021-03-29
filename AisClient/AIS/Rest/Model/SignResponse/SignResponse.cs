using Newtonsoft.Json;

namespace AIS.Rest.Model.SignResponse
{
    public class SignResponse
    {
        [JsonProperty("@RequestID")]
        public string RequestID { get; set; }

        [JsonProperty("@Profile")]
        public string Profile { get; set; }

        public Result Result { get; set; }

        public OptionalOutputs OptionalOutputs { get; set; }

        public SignatureObject SignatureObject { get; set; }
    }
}