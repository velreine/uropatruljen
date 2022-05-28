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

    public DeviceController(UroContext dbContext, DeviceRepository deviceRepository)
    {
        this._dbContext = dbContext;
        this._deviceRepository = deviceRepository;
    }
    
    [Authorize]
    [HttpGet("GetAuthenticatedUserDevices")]
    public IActionResult GetAuthenticatedUserDevices()
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

}