using Newtonsoft.Json;

namespace AIS.Rest.Model.SignResponse
{
    public class ScStepUpAuthorisationInfo
    {
        [JsonProperty("sc.Result")]
        public ScResult ScResult { get; set; }
    }
}