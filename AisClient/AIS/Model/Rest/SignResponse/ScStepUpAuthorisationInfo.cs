using Newtonsoft.Json;

namespace AIS.Model.Rest.SignResponse
{
    public class ScStepUpAuthorisationInfo
    {
        [JsonProperty("sc.Result")]
        public ScResult ScResult { get; set; }
    }
}