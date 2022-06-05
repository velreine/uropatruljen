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
    public class DeviceService : IDeviceService
    {
        private readonly RestClient _client;

        public DeviceService(RestClient client)
        {
            _client = client;
        }
        
        /// <summary>
        /// Fetches all devices the user has access to.
        /// Requires the user to be authenticated.
        /// </summary>
        /// <returns>A collection of Devices (possibly empty).</returns>
        public async Task<IEnumerable<AuthenticatedUserDevice>> GetUserDevices()
        {
            var request = new RestRequest("/Device/GetAuthenticatedUserDevices", Method.Get);

            var response = await _client.ExecuteGetAsync(request);

            if (!response.IsSuccessful || response.Content == null)
            {
                return Enumerable.Empty<AuthenticatedUserDevice>();
            }
            
            var data = JsonConvert.DeserializeObject<List<AuthenticatedUserDevice>>(response.Content);

            return data;
        }

        /// <summary>
        /// Registers a Device in the cloud.
        /// Requires the user to be authenticated.
        /// </summary>
        /// <param name="modelNumber">The model number to find the hardware layout by.</param>
        /// <param name="serialNumber">The unique serial number of the physical product.</param>
        /// <param name="homeId">The home in which to put the newly registered device.</param>
        /// <returns>True if registered successfully, false if not.</returns>
        public async Task<bool> RegisterDevice(string modelNumber, string serialNumber, int homeId)
        {
            
            var request = new RestRequest("/Device/RegisterDevice", Method.Post)
                .AddJsonBody(new
                {
                    modelNumber = modelNumber,
                    serialNumber = serialNumber,
                    registerToHomeId = homeId
                });

            var response = await _client.ExecutePostAsync(request);

            return response.IsSuccessful;
        }
        
    }
}