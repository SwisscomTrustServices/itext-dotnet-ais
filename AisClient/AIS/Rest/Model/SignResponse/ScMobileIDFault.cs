using Newtonsoft.Json;

namespace AIS.Rest.Model.SignResponse
{
    public class ScMobileIDFault
    {
        [JsonProperty("sc.Subcode")]
        public string ScSubcode { get; set; }

        [JsonProperty("sc.Reason")]
        public string ScReason { get; set; }

        [JsonProperty("sc.Detail")]
        public ScDetail ScDetail { get; set; }

    }
}