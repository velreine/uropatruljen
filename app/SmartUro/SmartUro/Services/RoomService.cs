using System;
using System.Threading.Tasks;
using CommonData.Model.DTO;
using RestSharp;
using SmartUro.Interfaces;

namespace SmartUro.Services
{
    public class RoomService : IRoomService
    {
        private readonly RestClient _client;

        public RoomService(RestClient client)
        {
            _client = client;
        }

        public async Task<CreateRoomResponseDTO> CreateRoom(CreateRoomRequestDTO room)
        {
            var request = new RestRequest("/Room/CreateRoom", Method.Post)
                .AddJsonBody(room);

            var response = await _client.ExecutePostAsync<CreateRoomResponseDTO>(request);

            if (!response.IsSuccessful || response.Content == null)
            {
                throw new Exception("It was not possible to create the room.");
            }

            return response.Data;
        }
        
        
    }
}