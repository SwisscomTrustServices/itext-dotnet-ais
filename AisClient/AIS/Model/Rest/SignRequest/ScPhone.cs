using Newtonsoft.Json;

namespace AIS.Model.Rest.SignRequest
{
    public class ScPhone
    {
        [JsonProperty("sc.Language")]
        public string ScLanguage { get; set; }

        [JsonProperty("sc.MSISDN")]
        public string ScMSISDN { get; set; }

        [JsonProperty("sc.Message")]
        public string ScMessage { get; set; }

        [JsonProperty("sc.SerialNumber")]
        public string ScSerialNumber { get; set; }
    }
}
