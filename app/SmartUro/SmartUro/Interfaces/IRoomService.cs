using System.Threading.Tasks;
using CommonData.Model.DTO;

namespace SmartUro.Interfaces
{
    public interface IRoomService
    {
        Task<CreateRoomResponseDTO> CreateRoom(CreateRoomRequestDTO room);
    }
}