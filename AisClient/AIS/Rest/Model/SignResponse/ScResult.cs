using Newtonsoft.Json;

namespace AIS.Rest.Model.SignResponse
{
    public class ScResult
    {
        [JsonProperty("sc.SerialNumber")]
        public string ScSerialNumber { get; set; }

        [JsonProperty("sc.ConsentURL")]
        public string ScConsentURL { get; set; }

        [JsonProperty("sc.MobileIDFault")]
        public ScMobileIDFault ScMobileIDFault { get; set; }
    }
}