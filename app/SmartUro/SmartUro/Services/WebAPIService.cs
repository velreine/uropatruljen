using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;
using SmartUro.Interfaces;
using RestSharp;
using RestSharp.Authenticators;
using CommonData.Model.Entity;

namespace SmartUro.Services
{
    internal class WebAPIService : IRestService<HardwareLayout>
    {
        private HttpClient _client;

        private readonly string _baseUrl;
        private readonly RestClient _restClient;

        public WebAPIService()
        {
            _client = new HttpClient();
            _baseUrl = "https://uroapp.dk";

            var options = new RestClientOptions(_baseUrl)
            {
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };

            _restClient = new RestClient(options);
        }

        public async Task ToggleState(int state)
        {
            Uri uri = new Uri(_baseUrl);

            HttpResponseMessage response = null;
            StringContent content = new StringContent(state.ToString());

            response = await _client.PostAsync(uri, content);

            if (response.IsSuccessStatusCode)
                Debug.WriteLine(@"\tSuccess!");
        }

        public async Task<bool> VerifyLogin(string _email, string _pass)
        {
            var request = new RestRequest("/auth", Method.Post)
                .AddJsonBody(new {
                    email = "nicky@example.com",
                    password = "12345"
                });

            var response = await _restClient.ExecutePostAsync(request);

            if (response.IsSuccessful)
            {
                Debug.WriteLine("LOGIN SUCCESS");
                return true;
            }
            else
            {
                Debug.WriteLine("LOGIN FAILED");
                return false;
            }
        }

        public Task<ICollection<HardwareLayout>> GetPairedUros()
        {
            throw new NotImplementedException();
        }
    }
}
