using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using CommonData.Model.DTO;
using CommonData.Model.Entity;

namespace SmartUro.Interfaces
{
    [SuppressMessage("ReSharper", "IdentifierTypo")]
    public interface IUserRegistrator
    {
        Task<RegisterResponseDTO> RegisterUserAsync(RegisterRequestDTO dto);
    }
}