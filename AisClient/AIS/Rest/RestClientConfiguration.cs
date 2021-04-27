using System.IO;
using AIS.Model;
using AIS.Utils;
using Newtonsoft.Json;

namespace AIS.Rest
{
    public class RestClientConfiguration
    {
        public string RestServiceSignUrl { get; set; }

        public string RestServicePendingUrl { get; set; }

        public string ClientKeyFile { get; set; }

        public string ClientKeyPassword { get; set; }

        public string ClientCertificateFile { get; set; }

        public int MaxConnectionsPerServer { get; set; }

        public int RequestTimeoutInSec { get; set; }

        public bool SkipServerCertificateValidation { get; set; }

        public RestClientConfiguration(string filePath)
        {
            string config = File.ReadAllText(filePath);
            LoadConfiguration(JsonConvert.DeserializeObject<ConfigurationProperties>(config));
        }

        public RestClientConfiguration(ConfigurationProperties configurationProperties)
        {
           LoadConfiguration(configurationProperties);
        }

        private void LoadConfiguration(ConfigurationProperties configuration)
        {
            RestServiceSignUrl = ConfigParser.GetStringNotNull("ServerRestSignUrl", configuration.ServerRestSignUrl);
            RestServicePendingUrl = ConfigParser.GetStringNotNull("ServerRestPendingUrl", configuration.ServerRestPendingUrl);
            ClientKeyFile = ConfigParser.GetStringNotNull("ClientAuthKeyFile", configuration.ClientAuthKeyFile);
            ClientKeyPassword = configuration.ClientAuthKeyPassword;
            ClientCertificateFile = ConfigParser.GetStringNotNull("ClientCertFile", configuration.ClientCertFile);
            MaxConnectionsPerServer = ConfigParser.GetIntNotNull("ClientHttpMaxConnectionsPerRoute", configuration.ClientHttpMaxConnectionsPerServer);
            RequestTimeoutInSec = ConfigParser.GetIntNotNull("ClientHttpResponseTimeoutInSeconds", configuration.ClientHttpRequestTimeoutInSeconds);
            SkipServerCertificateValidation = configuration.SkipServerCertificateValidation;

            ValidateFieldVales();
        }

        private void ValidateFieldVales()
        {
            Validator.AssertIntValueBetween(MaxConnectionsPerServer, 2, 100, "The MaxConnectionsPerRoute parameter of the REST client configuration must be between 2 and 100", null);
            Validator.AssertIntValueBetween(RequestTimeoutInSec, 2, 100, "The ResponseTimeoutInSec parameter of the REST client configuration must be between 2 and 100", null);
        }
    }

  
}
