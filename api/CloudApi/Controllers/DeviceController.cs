using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using CloudApi.Data;
using CloudApi.Repository;
using CommonData.Model.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace CloudApi.Controllers;

[ApiController]
[Route("[controller]")]
public class DeviceController : ControllerBase
{
    private readonly UroContext _dbContext;
    private readonly DeviceRepository _deviceRepository;
    private readonly HardwareLayoutRepository _layoutRepository;
    private readonly HomeRepository _homeRepository;

    public DeviceController(UroContext dbContext, DeviceRepository deviceRepository, HardwareLayoutRepository layoutRepository, HomeRepository homeRepository)
    {
        this._dbContext = dbContext;
        this._deviceRepository = deviceRepository;
        this._layoutRepository = layoutRepository;
        this._homeRepository = homeRepository;
    }
    
    [Authorize]
    [HttpGet("GetAuthenticatedUserDevices")]
    public ActionResult<IEnumerable<Device>> GetAuthenticatedUserDevices()
    {
        // Extracting person id from the token.
        var user = HttpContext.User;
        var UserIdClaim = user.FindFirst(c => c.Type == "PersonId")?.Value;

        if (UserIdClaim == null)
        {
            return BadRequest("Unable to authorize user.");
        }

        var userId = Convert.ToInt32(UserIdClaim);

        var devices = _deviceRepository.GetUserDevices(userId);

        return Ok(devices);
    }

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
        var device = new Device
        {
            Layout = layout,
            Name = layout.ProductName,
            SerialNumber = dto.SerialNumber,
            Home = home,
            Room = null // Room can be attached later in app, not a register time.
        };

        // Add the new device to the context.
        var savedDevice = _dbContext.Devices.Add(device).Entity;

        // Save the DB changes.
        _dbContext.SaveChanges();

        // Return the saved entity to the client.
        return Ok(savedDevice);
    }

    public class RegisterDeviceRequestDTO
    {
        public string ModelNumber { get; set; }
        public string SerialNumber { get; set; }
        public int RegisterToHomeId { get; set; }
    }
    

}