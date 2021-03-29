using Newtonsoft.Json;

namespace AIS.Rest.Model.SignRequest
{
    public class DsigDigestMethod
    {
        [JsonProperty("@Algorithm")]
        public string Algorithm { get; set; }
    }
}
