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

        public string ServerCertificateFile { get; set; }

        public int MaxTotalConnections { get; set; }

        public int MaxConnectionsPerRoute { get; set; }

        public int ConnectionTimeoutInSec { get; set; }

        public int ResponseTimeoutInSec { get; set; }

        public void SetFromConfigurationFile(string filePath)
        {
            string config = File.ReadAllText(filePath);
            SetFromConfiguration(JsonConvert.DeserializeObject<ConfigurationProperties>(config));
        }

        public void SetFromConfiguration(ConfigurationProperties configuration)
        {
            RestServiceSignUrl = ConfigParser.GetStringNotNull("ServerRestSignUrl", configuration.ServerRestSignUrl);
            RestServicePendingUrl = ConfigParser.GetStringNotNull("ServerRestPendingUrl", configuration.ServerRestPendingUrl);
            ClientKeyFile = ConfigParser.GetStringNotNull("ClientAuthKeyFile", configuration.ClientAuthKeyFile);
            ClientKeyPassword = configuration.ClientAuthKeyPassword;
            ClientCertificateFile = ConfigParser.GetStringNotNull("ClientCertFile", configuration.ClientCertFile);
            ServerCertificateFile = ConfigParser.GetStringNotNull("ServerCertFile", configuration.ServerCertFile);
            MaxTotalConnections = ConfigParser.GetIntNotNull("ClientHttpMaxTotalConnections", configuration.ClientHttpMaxTotalConnections);
            MaxConnectionsPerRoute = ConfigParser.GetIntNotNull("ClientHttpMaxConnectionsPerRoute", configuration.ClientHttpMaxConnectionsPerRoute);
            ConnectionTimeoutInSec = ConfigParser.GetIntNotNull("", configuration.ClientHttpConnectionTimeoutInSeconds);
            ResponseTimeoutInSec = ConfigParser.GetIntNotNull("ClientHttpResponseTimeoutInSeconds", configuration.ClientHttpResponseTimeoutInSeconds);

            ValidateFieldVales();
        }

        private void ValidateFieldVales()
        {
            Validator.AssertIntValueBetween(MaxTotalConnections, 2, 100, "The MaxTotalConnections parameter of the REST client configuration must be between 2 and 100", null);
            Validator.AssertIntValueBetween(MaxConnectionsPerRoute, 2, 100, "The MaxConnectionsPerRoute parameter of the REST client configuration must be between 2 and 100", null);
            Validator.AssertIntValueBetween(ConnectionTimeoutInSec, 2, 100, "The ConnectionTimeoutInSec parameter of the REST client configuration must be between 2 and 100", null);
            Validator.AssertIntValueBetween(ResponseTimeoutInSec, 2, 100, "The ResponseTimeoutInSec parameter of the REST client configuration must be between 2 and 100", null);
        }
    }

  
}
