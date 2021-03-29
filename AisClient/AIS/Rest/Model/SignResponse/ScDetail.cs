using Newtonsoft.Json;

namespace AIS.Rest.Model.SignResponse
{
    public class ScDetail
    {
        [JsonProperty("ns1.detail")]
        public string Ns1Detail { get; set; }

        [JsonProperty("ns2.UserAssistance")]
        public Ns2UserAssistance Ns2UserAssistance { get; set; }
    }
}