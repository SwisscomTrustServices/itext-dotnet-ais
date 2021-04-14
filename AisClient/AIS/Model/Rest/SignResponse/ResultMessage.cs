using Newtonsoft.Json;

namespace AIS.Model.Rest.SignResponse
{
    public class ResultMessage
    {
        [JsonProperty("@xml.lang")]
        public string XmlLang { get; set; }

        [JsonProperty("$")]
        public string S { get; set; }
    }
}
