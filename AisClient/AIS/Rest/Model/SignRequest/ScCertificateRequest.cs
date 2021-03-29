using Newtonsoft.Json;

namespace AIS.Rest.Model.SignRequest
{
    public class ScCertificateRequest
    {
        [JsonProperty("sc.DistinguishedName")]
        public string ScDistinguishedName { get; set; }

        [JsonProperty("sc.StepUpAuthorisation")]
        public ScStepUpAuthorisation ScStepUpAuthorisation { get; set; }
    }
}
