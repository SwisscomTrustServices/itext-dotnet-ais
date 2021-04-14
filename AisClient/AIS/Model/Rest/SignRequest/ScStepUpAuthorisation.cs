using Newtonsoft.Json;

namespace AIS.Model.Rest.SignRequest
{
    public class ScStepUpAuthorisation
    {
        [JsonProperty("sc.Phone")]
        public ScPhone ScPhone { get; set; }
    }
}
