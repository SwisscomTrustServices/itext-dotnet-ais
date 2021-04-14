using Newtonsoft.Json;

namespace AIS.Model.Rest.SignResponse
{
    public class ScRevocationInformation
    {
        [JsonProperty("sc.CRLs")]
        public ScCRLs ScCRLs { get; set; }

        [JsonProperty("sc.OCSPs")]
        public ScOCSPs ScOCSPs { get; set; }
    }
}