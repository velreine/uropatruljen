using System.Collections.Generic;
using System.Threading.Tasks;
using CommonData.Model.Entity;

namespace SmartUro.Interfaces
{
    public interface IDeviceService
    {
        Task<IEnumerable<Device>> GetUserDevices();
        Task<bool> RegisterDevice(string modelNumber, string serialNumber, int homeId);
    }
}