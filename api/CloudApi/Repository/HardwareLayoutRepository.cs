using CloudApi.Data;
using CommonData.Model.Entity;
using Microsoft.EntityFrameworkCore;

namespace CloudApi.Repository;

public class HardwareLayoutRepository
{
    private readonly UroContext _dbContext;

    public HardwareLayoutRepository(UroContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public HardwareLayout? Find(int id)
    {
        return _dbContext.HardwareLayouts
                .Include(layout => layout.AttachedComponents)
                .ThenInclude(component => component.Pins)
                .FirstOrDefault(layout => layout.Id == id)
            ;
    }

    public HardwareLayout? FindByModelNumber(string modelNumber)
    {
        return _dbContext.HardwareLayouts
                .Include(layout => layout.AttachedComponents)
                .ThenInclude(component => component.Pins)
                .FirstOrDefault(layout => layout.ModelNumber == modelNumber)
            ;
    }
    
}