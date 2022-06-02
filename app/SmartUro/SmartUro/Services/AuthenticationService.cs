using System;
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

        private Person _lastAuthenticatedUser;
        
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

        public Task Logout()
        {
            // Remove the current authenticator from the rest client.
            _restClient.Authenticator = null;
            _lastAuthenticatedUser = null;

            // Update the state to indicate we are no longer authenticated.
            IsAuthenticated = false;
            
            return Task.CompletedTask;
        }

        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
        private struct WhoAmIResponse
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public string Email { get; set; }
        }
        
        public async Task<Person> GetAuthenticatedUser()
        {
            // If the user is not authenticated yet, then return null.
            if (IsAuthenticated == false) return null;

            // If we already know who the authenticated user is, return that.
            if (_lastAuthenticatedUser != null) return _lastAuthenticatedUser;
            
            // Otherwise do a roundtrip to the cloud api for figuring out who we are.
            var request = new RestRequest("/WhoAmI", Method.Get);

            var response = await _restClient.ExecuteGetAsync(request);

            if (!response.IsSuccessful || response.Content == null)
            {
                throw new Exception("Unable to get the currently authenticated user.");
            }

            var data = JsonConvert.DeserializeObject<WhoAmIResponse>(response.Content);

            _lastAuthenticatedUser = new Person()
            {
                Id = data.Id,
                Email = data.Email,
                Name = data.Name,
            };

            return _lastAuthenticatedUser;
        }
    }
}