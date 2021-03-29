using Newtonsoft.Json;

namespace AIS.Rest.Model.PendingRequest
{
    public class AsyncPendingRequest
    {
        [JsonProperty("@Profile")]
        public string Profile { get; set; }

        public OptionalInputs OptionalInputs { get; set; }
    }
}
