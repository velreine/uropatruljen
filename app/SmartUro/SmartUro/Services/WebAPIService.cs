using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SmartUro.Services
{
    internal class WebAPIService
    {
        HttpClient client;

        public WebAPIService()
        {
            client = new HttpClient();
        }

        public async Task OnOff()
        {
            Uri uri = new Uri("web api address");

            HttpResponseMessage response = null;
            StringContent content = new StringContent("OnOff");

            response = await client.PostAsync(uri, content);

            if(response.IsSuccessStatusCode)
            {
                Debug.WriteLine(@"\tSuccess!");
            }
        }
    }
}
