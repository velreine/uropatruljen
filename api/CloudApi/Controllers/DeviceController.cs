using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using CloudApi.Data;
using CloudApi.Repository;
using CommonData.Model.DTO;
using CommonData.Model.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace CloudApi.Controllers;

/// <summary>
/// This controller provides endpoints for retrieving/manipulating Devices.
/// </summary>
[ApiController]
[Route("[controller]")]
public class DeviceController : AbstractController
{
    private readonly UroContext _dbContext;
    private readonly DeviceRepository _deviceRepository;
    private readonly HardwareLayoutRepository _layoutRepository;
    private readonly HomeRepository _homeRepository;

    /// <summary>
    /// The constructor for the DeviceController, dependencies are resolved and injected by the framework.
    /// </summary>
    /// <param name="dbContext">Database context.</param>
    /// <param name="deviceRepository">Device Repository</param>
    /// <param name="layoutRepository">HardwareLayout Repository</param>
    /// <param name="homeRepository">Home Repository</param>
    /// <param name="personRepository">Person Repository</param>
    public DeviceController(UroContext dbContext, DeviceRepository deviceRepository, HardwareLayoutRepository layoutRepository, HomeRepository homeRepository, PersonRepository personRepository) : base(personRepository)
    {
        _dbContext = dbContext;
        _deviceRepository = deviceRepository;
        _layoutRepository = layoutRepository;
        _homeRepository = homeRepository;
    }
    
    /// <summary>
    /// This endpoint returns all the devices that the currently authenticated user has access to.
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet("GetAuthenticatedUserDevices")]
    public ActionResult<IEnumerable<AuthenticatedUserDevice>> GetAuthenticatedUserDevices()
    {
        // Extracting person id from the token.
        var personId = GetAuthenticatedUserId();
        
        if (personId == null)
        {
            return BadRequest("Unable to authorize user.");
        }
        
        // Transform the data from EF to our desired format.
        var devices = _deviceRepository.GetUserDevices((int)personId)
            .Select(entityDevice =>
                new AuthenticatedUserDevice(
                    entityDevice.Id,
                    entityDevice.SerialNumber,
                    entityDevice.Name,
                    entityDevice.HomeId,
                    entityDevice.HardwareLayoutId,
                    entityDevice.RoomId)
            );
        
        return Ok(devices);
    }

    /// <summary>
    /// This is the main endpoint that will be invoked when a device should be registered.
    /// </summary>
    [AllowAnonymous]
    [HttpPost("RegisterDevice")]
    public ActionResult RegisterDevice([FromBody] RegisterDeviceRequestDTO dto)
    {
     
        // TODO: Add [Authorize] attribute on this endpoint,
        // The register should happen through the app, which should be logged in.
        // This way we can ensure the device is registered to the currently logged in user.
        
        // 1. Lookup hardware layout by model number ensuring it exists.
        var layout = _layoutRepository.FindByModelNumber(dto.ModelNumber);

        if (layout == null)
        {
            return BadRequest("Layout with given model number not found, could not register device.");
        }
        
        // 2. Lookup the home, ensuring it exists.
        var home = _homeRepository.Find(dto.RegisterToHomeId);

        if (home == null)
        {
            return BadRequest("Home with given id not found, could not register device.");
        }
        
        
        // 3. Prefill some data on the device.
        var device = new Device(
            layout.ProductName,
            dto.SerialNumber!,
            layout,
            home,
            null
        );
        
        // Add the new device to the context.
        var savedDevice = _dbContext.Devices.Add(device).Entity;

        // Save the DB changes.
        _dbContext.SaveChanges();

        // Return the saved entity to the client.
        return Ok(savedDevice);
    }


    

}