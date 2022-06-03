using System.Collections.Generic;
using System.Threading.Tasks;
using CommonData.Model.Entity;

namespace SmartUro.Interfaces
{
    public interface IHomeService
    {
        Task<IEnumerable<Home>> GetUserHomes();
    }
}