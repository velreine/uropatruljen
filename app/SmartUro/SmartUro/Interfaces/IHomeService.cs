using System.Collections.Generic;
using System.Threading.Tasks;
using CommonData.Model.DTO;

namespace SmartUro.Interfaces
{
    public interface IHomeService
    {
        Task<IEnumerable<AuthenticatedUserHomeResponseDTO>> GetUserHomes();

        Task<CreateHomeResponseDTO> CreateHome(CreateHomeRequestDTO home);

        Task<UpdateHomeResponseDTO> UpdateHome(UpdateHomeRequestDTO home);

        Task<bool> DeleteHome(int id);
    }
}