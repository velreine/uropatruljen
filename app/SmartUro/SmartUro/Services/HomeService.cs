using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using CommonData.Model.Entity;
using Newtonsoft.Json;
using RestSharp;
using SmartUro.Interfaces;

namespace SmartUro.Services
{
    public class HomeService : IHomeService
    {
        private readonly RestClient _client;

        public HomeService(RestClient client)
        {
            _client = client;
        }
        
        public async Task<IEnumerable<Home>> GetUserHomes()
        {
            var request = new RestRequest("/Home/GetAuthenticatedUserHomes", Method.Get);

            var response = await _client.ExecuteGetAsync(request);

            if (!response.IsSuccessful || response.Content == null)
            {
                return ImmutableArray<Home>.Empty;
            }
            
            var data = JsonConvert.DeserializeObject<List<Home>>(response.Content);

            return data;
        }
    }
}