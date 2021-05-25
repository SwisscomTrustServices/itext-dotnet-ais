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

        public string LicenseFilePath { get; set; }


        public AisClientConfiguration(ConfigurationProperties properties)
        {
            LoadConfiguration(properties);
        }
        public AisClientConfiguration(string filePath)
        {
            string config = File.ReadAllText(filePath);
            LoadConfiguration(JsonConvert.DeserializeObject<ConfigurationProperties>(config));
        }

        private void LoadConfiguration(ConfigurationProperties configuration)
        {

            SignaturePollingIntervalInSeconds = ConfigParser.GetIntNotNull("ClientPollIntervalInSeconds", configuration.ClientPollIntervalInSeconds);
            SignaturePollingRounds = ConfigParser.GetIntNotNull("ClientPollRounds", configuration.ClientPollRounds);
            LicenseFilePath = configuration.ITextLicenseFilePath;
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
