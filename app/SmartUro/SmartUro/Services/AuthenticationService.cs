using System.Buffers;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CommonData.Model.Entity;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using SmartUro.Interfaces;

// ReSharper disable IdentifierTypo

namespace SmartUro.Services
{
    public class AuthenticationService : IUserRegistrator, IUserAuthenticator
    {
        private readonly RestClient _restClient;
        public bool IsAuthenticated { get; set; } = false;

        public AuthenticationService(RestClient restClient)
        {
            _restClient = restClient;
        }

        public async Task<bool> RegisterUserAsync(Person person, string plainTextPassword)
        {
            var request = new RestRequest("/register", Method.Post)
                .AddJsonBody(new
                {
                    name = person.Name,
                    email = person.Email,
                    password = plainTextPassword
                });

            // TODO: Maybe return the token etc...
            
            var response = await _restClient.ExecutePostAsync(request);
            
            return response.IsSuccessful;
        }

        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
        private struct LoginResponse
        {
            public string Message { get; set; }
            public string Token { get; set; }
        } 
        
        public async Task<bool> Login(string email, string plainTextPassword)
        {
            var request = new RestRequest("/auth", Method.Post)
                .AddJsonBody(new {
                    email = "nicky@example.com",
                    password = "12345"
                });

            var response = await _restClient.ExecutePostAsync(request);
            
            if (!response.IsSuccessful || response.Content == null)
            {
                IsAuthenticated = false;
                return false;
            }

            var data = JsonConvert.DeserializeObject<LoginResponse>(response.Content);
            
            // Add a bearer token to the Rest Client upon successful login.
            _restClient.Authenticator = new JwtAuthenticator(data.Token);
            IsAuthenticated = true;
            return true;
        }

        
    }
}