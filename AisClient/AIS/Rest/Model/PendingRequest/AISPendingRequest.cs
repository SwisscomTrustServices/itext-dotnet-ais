using Newtonsoft.Json;

namespace AIS.Rest.Model.PendingRequest
{
    public class AISPendingRequest
    {
        [JsonProperty("async.PendingRequest")]
        public AsyncPendingRequest AsyncPendingRequest { get; set; }
    }
}
