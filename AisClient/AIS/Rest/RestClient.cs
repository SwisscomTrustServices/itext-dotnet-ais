using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AIS.Rest.Model.PendingRequest;
using AIS.Rest.Model.SignRequest;
using AIS.Rest.Model.SignResponse;
using AIS.Utils;
using X509Certificate = System.Security.Cryptography.X509Certificates.X509Certificate;

namespace AIS.Rest
{
    public class RestClient : IRestClient
    {
        public RestClient()
        {
            ServicePointManager.ServerCertificateValidationCallback +=
                (sender, cert, chain, sslPolicyErrors) => true; // TODO add validation instead of ignore
        }

        public AISSignResponse RequestSignature(AISSignRequest request, Trace trace)
        {
            CertificateLoader certificateLoader = new CertificateLoader();

            X509Certificate2 certWithKey = certificateLoader.LoadCertificate(new CertificateConfiguration());
            HttpClient client = GetHttpClient(certWithKey);
            var task = Task.Run(() => SendCall(GetRequestContent(), client));
            task.Wait();
            return new AISSignResponse();
        }

        private string GetRequestContent()
        {
            return File.ReadAllText("input.txt");

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
            string url = "https://ais.swisscom.com/AIS-Server/rs/v1.0/sign";
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new StringContent(jsonObj, Encoding.UTF8, "application/json");
            string result = string.Empty;
            try
            {
                var crossCoreRequestTimestamp = DateTime.Now;

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
