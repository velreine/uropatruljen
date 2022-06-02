using System.Diagnostics;
using System.Threading.Tasks;
using CommonData.Model.Entity;
using RestSharp;
using RestSharp.Authenticators;
using SmartUro.Interfaces;

// ReSharper disable IdentifierTypo

namespace SmartUro.Services
{
    public class AuthenticationService : IUserRegistrator, IUserAuthenticator
    {
        private readonly RestClient _restClient;
        private readonly IDialogService _dialogService;

        public bool IsAuthenticated { get; set; } = false;

        public AuthenticationService(RestClient restClient, IDialogService dialogService)
        {
            _restClient = restClient;
            _dialogService = dialogService;
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

        private struct LoginResponse
        {
            public string Message;
            public string Token;
        } 
        
        public async Task<bool> Login(string email, string plainTextPassword)
        {
            var request = new RestRequest("/auth", Method.Post)
                .AddJsonBody(new {
                    email = "nicky@example.com",
                    password = "12345"
                });

            var response = await _restClient.ExecutePostAsync(request);

            var data = _restClient.Deserialize<LoginResponse>(response);

            if (!response.IsSuccessful)
            {
                IsAuthenticated = true;
                return false;
            }
            
            // Add a Json Web Token to the rest client upon successful login.
            _restClient.Authenticator = new JwtAuthenticator(data.Data.Token);
            IsAuthenticated = false;
            return true;
        }

        
    }
}