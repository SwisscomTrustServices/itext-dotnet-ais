using Newtonsoft.Json;

namespace AIS.Model.Rest.SignResponse
{
    public class OptionalOutputs
    {
        [JsonProperty("async.ResponseID")]
        public string AsyncResponseID { get; set; }

        [JsonProperty("sc.APTransID")]
        public string ScAPTransID { get; set; }

        [JsonProperty("sc.StepUpAuthorisationInfo")]
        public ScStepUpAuthorisationInfo ScStepUpAuthorisationInfo { get; set; }

        [JsonProperty("sc.RevocationInformation")]
        public ScRevocationInformation ScRevocationInformation { get; set; }
    }
}