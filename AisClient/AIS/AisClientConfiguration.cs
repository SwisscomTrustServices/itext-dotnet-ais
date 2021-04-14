using System.IO;
using AIS.Model;
using AIS.Utils;
using Newtonsoft.Json;

namespace AIS
{
    public class AisClientConfiguration
    {
        public int SignaturePollingIntervalInSeconds { get; set; }

        public int SignaturePollingRounds { get; set; }

        public void SetFromConfigurationFile(string filePath)
        {
            string config = File.ReadAllText(filePath);
            SetFromConfiguration(JsonConvert.DeserializeObject<ConfigurationProperties>(config));
        }

        public void SetFromConfiguration(ConfigurationProperties configuration)
        {

            SignaturePollingIntervalInSeconds = ConfigParser.GetIntNotNull("ClientPollIntervalInSeconds", configuration.ClientPollIntervalInSeconds);
            SignaturePollingRounds = ConfigParser.GetIntNotNull("ClientPollRounds", configuration.ClientPollRounds);

            ValidateFieldVales();
        }

        private void ValidateFieldVales()
        {
            Validator.AssertIntValueBetween(SignaturePollingIntervalInSeconds, 1, 300, "The SignaturePollingIntervalInSeconds parameter of the AIS client " +
                                                                                       "configuration must be between 1 and 300 seconds", null);
            Validator.AssertIntValueBetween(SignaturePollingRounds, 1, 100, "The SignaturePollingRounds parameter of the AIS client "
                                                                            + "configuration must be between 1 and 100 rounds", null);

        }
    }
}
