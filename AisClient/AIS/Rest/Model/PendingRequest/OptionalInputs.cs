using Newtonsoft.Json;

namespace AIS.Rest.Model.PendingRequest
{
    public class OptionalInputs
    {
        public ClaimedIdentity ClaimedIdentity { get; set; }

        [JsonProperty("async.ResponseID")]
        public string AsyncResponseId { get; set; }
    }
}
