using System.Collections.Generic;
using System.Threading.Tasks;
using CommonData.Model.Entity;

namespace SmartUro.Interfaces
{
    public interface IHardwareLayoutService
    {
        Task<IEnumerable<HardwareLayout>> GetUserHardwareLayouts();
    }
}