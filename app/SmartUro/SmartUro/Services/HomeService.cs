using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using CommonData.Model.DTO;
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
        
        public async Task<IEnumerable<AuthenticatedUserHomeResponseDTO>> GetUserHomes()
        {
            var request = new RestRequest("/Home/GetAuthenticatedUserHomes", Method.Get);

            var response = await _client.ExecuteGetAsync(request);

            if (response.Content == null || !response.IsSuccessful)
            {
                return Enumerable.Empty<AuthenticatedUserHomeResponseDTO>();
            }
            
            var data = JsonConvert.DeserializeObject<IEnumerable<AuthenticatedUserHomeResponseDTO>>(response.Content);

            return data;
        }

        public async Task<CreateHomeResponseDTO> CreateHome(CreateHomeRequestDTO home)
        {
            var request = new RestRequest("/Home/CreateHome", Method.Post)
                .AddJsonBody(home);

            var response = await _client.ExecutePostAsync<CreateHomeResponseDTO>(request);

            if (!response.IsSuccessful || response.Content == null)
            {
                throw new Exception("It was not possible to create the home.");
            }

            return response.Data;
        }
        
    }
}