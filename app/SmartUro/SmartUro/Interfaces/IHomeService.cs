using System.Collections.Generic;
using System.Threading.Tasks;
using CommonData.Model.DTO;

namespace SmartUro.Interfaces
{
    public interface IHomeService
    {
        Task<IEnumerable<AuthenticatedUserHome>> GetUserHomes();
    }
}