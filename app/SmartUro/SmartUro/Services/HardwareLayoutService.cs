using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonData.Model.DTO;
using CommonData.Model.Entity;
using Newtonsoft.Json;
using RestSharp;
using SmartUro.Interfaces;

namespace SmartUro.Services
{
    public class HardwareLayoutService : IHardwareLayoutService
    {
        private readonly RestClient _client;

        public HardwareLayoutService(RestClient client)
        {
            _client = client;
        }

        public async Task<IEnumerable<HardwareLayout>> GetUserHardwareLayouts()
        {
            var request = new RestRequest("/HardwareLayout/GetUserHardwareLayouts", Method.Get);

            var response = await _client.ExecuteGetAsync(request);

            if (!response.IsSuccessful || response.Content == null)
            {
                return Enumerable.Empty<HardwareLayout>();
            }
            
            var data = JsonConvert.DeserializeObject<List<HardwareLayout>>(response.Content);

            return data;
        }
        
    }
}