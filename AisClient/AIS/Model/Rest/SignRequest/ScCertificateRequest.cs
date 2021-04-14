using Newtonsoft.Json;

namespace AIS.Model.Rest.SignRequest
{
    public class ScCertificateRequest
    {
        [JsonProperty("sc.DistinguishedName")]
        public string ScDistinguishedName { get; set; }

        [JsonProperty("sc.StepUpAuthorisation")]
        public ScStepUpAuthorisation ScStepUpAuthorisation { get; set; }
    }
}
