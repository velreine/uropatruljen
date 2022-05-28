using CloudApi.Data;
using CommonData.Model.Entity;
using Microsoft.EntityFrameworkCore;

namespace CloudApi.Repository;

public class DeviceRepository
{
    
    private readonly UroContext _dbContext;


    public DeviceRepository(UroContext dbContext)
    {
        this._dbContext = dbContext;
    }
    
    public IEnumerable<Device> GetUserDevices(int userId)
    {
        var devices = 
            _dbContext
                .Devices
                .Where(device => device.Room.Home.Residents.Any(p => p.Id == userId))
                .Include(x => x.Layout);
        
        return devices;
    } 
    
}