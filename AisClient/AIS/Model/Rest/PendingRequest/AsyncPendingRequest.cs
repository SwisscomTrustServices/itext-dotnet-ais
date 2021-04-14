using Newtonsoft.Json;

namespace AIS.Model.Rest.PendingRequest
{
    public class AsyncPendingRequest
    {
        [JsonProperty("@Profile")]
        public string Profile { get; set; }

        public OptionalInputs OptionalInputs { get; set; }
    }
}
