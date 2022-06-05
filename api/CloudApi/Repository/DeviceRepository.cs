using CloudApi.Data;
using CommonData.Model.Entity;
using Microsoft.EntityFrameworkCore;

namespace CloudApi.Repository;

/// <summary>
/// Provides convenience methods for retrieving devices based on different criteria.
/// </summary>
public class DeviceRepository
{
    
    private readonly UroContext _dbContext;
    
    /// <summary>
    /// The constructor for the DeviceRepository, the database context is injected by the framework.
    /// </summary>
    public DeviceRepository(UroContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    /// <summary>
    /// Given a user/person id retrieves the devices that the person has access to control.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public IEnumerable<Device> GetUserDevices(int userId)
    {
        var devices = 
            _dbContext
                .Devices
                .Where(device => device.Home.Persons.Any(person => person.Id == userId))
                //.Include(d => d.Home)
                //.Include(d => d.Layout)
                //.ThenInclude(l => l.AttachedComponents)
                //.ThenInclude(ac => ac.Pins)
                
            ;
        
        return devices;
    } 
    
}