using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AIS.Model.Rest.PendingRequest;
using AIS.Model.Rest.SignRequest;
using AIS.Model.Rest.SignResponse;
using AIS.Utils;
using Newtonsoft.Json;
using X509Certificate = System.Security.Cryptography.X509Certificates.X509Certificate;

namespace AIS.Rest
{
    public class RestClient : IRestClient
    {
        private RestClientConfiguration configuration;
        private HttpClient client;

        public RestClient()
        {
            ServicePointManager.ServerCertificateValidationCallback +=
                (sender, cert, chain, sslPolicyErrors) => true; // TODO add validation instead of ignore
        }

        public void SetConfiguration(RestClientConfiguration configuration)
        {
            this.configuration = configuration;
            client = BuildClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        private HttpClient BuildClient()
        {
            CertificateLoader certificateLoader = new CertificateLoader();

            X509Certificate2 certWithKey = certificateLoader.LoadCertificate(new CertificateConfiguration
            {
                CertificateFile = configuration.ClientCertificateFile,
                Password = configuration.ClientKeyPassword,
                PrivateKeyFile = configuration.ClientKeyFile
            });
            return GetHttpClient(certWithKey);
        }

        public AISSignResponse RequestSignature(AISSignRequest request, Trace trace)
        {
          
            var task = Task.Run(() => SendCall(GetRequestContent(request), client));
            task.Wait();
            return JsonConvert.DeserializeObject<AISSignResponse>(task.Result);
        }

        private string GetRequestContent(AISSignRequest request)
        {
            return JsonConvert.SerializeObject(request, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }




        public AISSignResponse PollForSignatureStatus(AISPendingRequest request, Trace trace)
        {
            throw new NotImplementedException();
        }


        private static HttpClient GetHttpClient(X509Certificate cert)
        {
            HttpClientHandler handler = new HttpClientHandler();

            handler.ClientCertificates.Add(cert);
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;

            return new HttpClient(handler);
        }




        private async Task<string> SendCall(string jsonObj, HttpClient client)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, configuration.RestServiceSignUrl);
            request.Content = new StringContent(jsonObj, Encoding.UTF8, "application/json");
            string result = string.Empty;
            try
            {

                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    throw new Exception(response.StatusCode.ToString());
                }

                return result;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
