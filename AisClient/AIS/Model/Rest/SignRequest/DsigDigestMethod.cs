using Newtonsoft.Json;

namespace AIS.Model.Rest.SignRequest
{
    public class DsigDigestMethod
    {
        [JsonProperty("@Algorithm")]
        public string Algorithm { get; set; }
    }
}
