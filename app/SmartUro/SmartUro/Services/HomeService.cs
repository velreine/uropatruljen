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
        
        public async Task<IEnumerable<AuthenticatedUserHome>> GetUserHomes()
        {
            var request = new RestRequest("/Home/GetAuthenticatedUserHomes", Method.Get);

            var response = await _client.ExecuteGetAsync<IEnumerable<AuthenticatedUserHome>>(request);

            /*if (!response.IsSuccessful || response.Content == null)
            {
                return Enumerable.Empty<AuthenticatedUserHome>();
            }*/
            
            //var data = JsonConvert.DeserializeObject<IEnumerable<AuthenticatedUserHome>>(response.Content);

            return response.Data;
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