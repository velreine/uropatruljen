using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;
using SmartUro.Interfaces;

namespace SmartUro.Services
{
    internal class WebAPIService : IRestService
    {
        private HttpClient _client;
        private string _baseUri;

        public WebAPIService()
        {
            _client = new HttpClient();
            _baseUri = "base address";
        }

        public async Task ToggleStateAsync(int state)
        {
            Uri uri = new Uri(_baseUri);

            HttpResponseMessage response = null;
            StringContent content = new StringContent(state.ToString());

            response = await _client.PostAsync(uri, content);

            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine(@"\tSuccess!");
            }
        }
    }
}
