using System.Threading.Tasks;
using CommonData.Model.DTO;

namespace SmartUro.Interfaces
{
    public interface IRoomService
    {
        Task<CreateRoomResponseDTO> CreateRoom(CreateRoomRequestDTO room);

        Task<UpdateRoomResponseDTO> UpdateRoom(UpdateRoomRequestDTO dto);

        Task<bool> DeleteRoom(int id);
    }
}