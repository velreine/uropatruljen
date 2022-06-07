using System;
using System.Threading.Tasks;
using CommonData.Model.DTO;
using Newtonsoft.Json;
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

        public async Task<UpdateRoomResponseDTO> UpdateRoom(UpdateRoomRequestDTO dto)
        {
            // Construct the request.
            var request = new RestRequest("/Room/UpdateRoom", Method.Put)
                .AddJsonBody(dto);

            // Send the PUT request.
            var response = await _client.ExecutePutAsync<UpdateRoomResponseDTO>(request);

            // If the response is bad, throw.
            if (!response.IsSuccessful || response.Content == null)
            {
                throw new Exception("It was not possible to update the room.");
            }

            // Otherwise, deserialize the response.
            var data = JsonConvert.DeserializeObject<UpdateRoomResponseDTO>(response.Content);

            // And return it to the consumer.
            return data;
        }

        public async Task<bool> DeleteRoom(int id)
        {
            // Construct the request.
            var request = new RestRequest("/Room/DeleteRoom", Method.Delete)
                .AddParameter("id", id);
            
            // Send the DELETE request.
            var response = await _client.ExecuteAsync(request);

            // If the response is successful the home was deleted.
            return response.IsSuccessful;
        }
        
        
    }
}