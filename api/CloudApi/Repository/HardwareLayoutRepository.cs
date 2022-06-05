using System.Diagnostics.CodeAnalysis;
using CloudApi.Data;
using CommonData.Model.Entity;
using Microsoft.EntityFrameworkCore;

namespace CloudApi.Repository;

/// <summary>
/// This repository provides methods for retrieving HardwareLayouts based on different criteria.
/// </summary>
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class HardwareLayoutRepository
{
    private readonly UroContext _dbContext;

    /// <summary>
    /// The constructor for the HardwareLayoutRepository, the framework must provide the context of our Database.
    /// </summary>
    /// <param name="dbContext">Our database context.</param>
    public HardwareLayoutRepository(UroContext dbContext)
    {
        this._dbContext = dbContext;
    }

    /// <summary>
    /// Finds a hardware layout by its id.
    /// </summary>
    /// <param name="id">The id to look for.</param>
    /// <returns>The HardwareLayout.</returns>
    public HardwareLayout? Find(int id)
    {
        return _dbContext.HardwareLayouts
                .Include(layout => layout.Components)
                .ThenInclude(component => component.Pins)
                .FirstOrDefault(layout => layout.Id == id)
            ;
    }

    /// <summary>
    /// Find a hardware layout by its model number.
    /// </summary>
    /// <param name="modelNumber">The model number to match.</param>
    /// <returns>The HardwareLayout.</returns>
    public HardwareLayout? FindByModelNumber(string? modelNumber)
    {
        return _dbContext.HardwareLayouts
                .Include(layout => layout.Components)
                .ThenInclude(component => component.Pins)
                .FirstOrDefault(layout => layout.ModelNumber == modelNumber)
            ;
    }

    /// <summary>
    /// Find all layouts that the the user uses.
    /// </summary>
    /// <param name="personId">The id of the person for whom to find all those layouts.</param>
    /// <returns>The Layouts used by all the devices the person can control.</returns>
    public IEnumerable<HardwareLayout> GetAllUserDeviceLayouts(int personId)
    {

        // Finds all layouts, used by all devices, of all homes that the person resides in.
        return _dbContext.HardwareLayouts
                .Include(layout => layout.Components)
                .ThenInclude(component => component.Pins)
                .Where(layout =>
                    layout.Devices
                        .Any(device =>
                            device.Home.Persons
                                .Any(person => person.Id == personId)))
            ;
    }

}