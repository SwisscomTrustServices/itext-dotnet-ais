using Newtonsoft.Json;

namespace AIS.Model.Rest.PendingRequest
{
    public class AISPendingRequest
    {
        [JsonProperty("async.PendingRequest")]
        public AsyncPendingRequest AsyncPendingRequest { get; set; }
    }
}
