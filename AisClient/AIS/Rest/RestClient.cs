using System;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AIS.Common;
using AIS.Model.Rest.PendingRequest;
using AIS.Model.Rest.SignRequest;
using AIS.Model.Rest.SignResponse;
using AIS.Utils;
using Common.Logging;
using Newtonsoft.Json;

namespace AIS.Rest
{
    public class RestClient : IRestClient
    {
        private static  ILog logger = LogManager.GetLogger<RestClient>();
        private static string signRequestOperation = "SignRequest";
        private static string pendingRequestOperation = "PendingRequest";
        private RestClientConfiguration configuration;
        private HttpClient client;

        public RestClient(RestClientConfiguration restClientConfiguration)
        {
            SetConfiguration(restClientConfiguration);
        }
        private void SetConfiguration(RestClientConfiguration restClientConfiguration)
        {
            configuration = restClientConfiguration;
            client = BuildHttpClient();
            ConfigureConnections();
            if (configuration.SkipServerCertificateValidation)
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            }
        }

        private void ConfigureConnections()
        {
            if (configuration.SkipServerCertificateValidation)
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            }
        }

        private  HttpClient BuildHttpClient()
        {
            CertificateLoader certificateLoader = new CertificateLoader();

            X509Certificate2 certWithKey = certificateLoader.LoadCertificate(new CertificateConfiguration
            {
                CertificateFile = configuration.ClientCertificateFile,
                Password = configuration.ClientKeyPassword,
                PrivateKeyFile = configuration.ClientKeyFile
            });
            HttpClientHandler handler = new HttpClientHandler
            {
                MaxConnectionsPerServer = configuration.MaxConnectionsPerServer,
            };

            handler.ClientCertificates.Add(certWithKey);
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;

            var httpClient = new HttpClient(handler);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.Timeout = TimeSpan.FromSeconds(configuration.RequestTimeoutInSec);
            return httpClient;
        }

        public  AISSignResponse RequestSignature(AISSignRequest request, Trace trace)
        {
            return RequestAsync(request, configuration.RestServiceSignUrl, signRequestOperation, trace);
        }

        public  AISSignResponse PollForSignatureStatus(AISPendingRequest request, Trace trace)
        {
            return RequestAsync(request, configuration.RestServicePendingUrl, pendingRequestOperation, trace);
        }

        private  AISSignResponse RequestAsync<T>(T request, string serviceUrl, string operationName, Trace trace)
        {
            logger.Debug($"{operationName}: Serializing type object {request.GetType().Name} to JSON - {trace.Id}");
            var serializedRequest = SerializeRequest(request);
            var task = Task.Run(() => ExecuteRequest(serializedRequest, serviceUrl, operationName, trace));
            task.Wait();
            return JsonConvert.DeserializeObject<AISSignResponse>(task.Result);
        }
        private  string SerializeRequest<T>(T request)
        {
            return JsonConvert.SerializeObject(request, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        private  async Task<string> ExecuteRequest(string payload, string url, string operationName, Trace trace)
        {
            logger.Info($"{operationName}: Sending request to: [{url}] - {trace.Id}");
            logger.Debug($"{operationName}: Sending JSON to: [{url}], content: [{payload}] - {trace.Id}");
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new StringContent(payload, Encoding.UTF8, "application/json");
            try
            {

                var response = await client.SendAsync(request);
                logger.Info($"{operationName}: Received HTTP status code: {response.StatusCode} - {trace.Id}");
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    logger.Debug($"{operationName}: Received JSON content: {result} - {trace.Id}");
                    return result;
                }
                throw new AisClientException(response.StatusCode.ToString());

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
