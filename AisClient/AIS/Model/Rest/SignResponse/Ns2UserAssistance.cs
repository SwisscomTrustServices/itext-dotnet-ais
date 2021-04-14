using Newtonsoft.Json;

namespace AIS.Model.Rest.SignResponse
{
    public class Ns2UserAssistance
    {
        [JsonProperty("ns2.PortalUrl")]
        public string Ns2PortalUrl { get; set; }
    }
}
