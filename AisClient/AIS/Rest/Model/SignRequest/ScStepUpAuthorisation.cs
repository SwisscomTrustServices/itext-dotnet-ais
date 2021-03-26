using Newtonsoft.Json;

namespace AIS.Rest.Model.SignRequest
{
    public class ScStepUpAuthorisation
    {
        [JsonProperty("sc.Phone")]
        public ScPhone ScPhone { get; set; }
    }
}
