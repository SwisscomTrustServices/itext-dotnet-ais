using Newtonsoft.Json;

namespace AIS.Rest.Model.SignResponse
{
    public class ScRevocationInformation
    {
        [JsonProperty("sc.CRLs")]
        public ScCRLs ScCRLs { get; set; }

        [JsonProperty("sc.OCSPs")]
        public ScOCSPs ScOCSPs { get; set; }
    }
}