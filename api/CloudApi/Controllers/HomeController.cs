using CloudApi.Data;
using CloudApi.Repository;
using CommonData.Model.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudApi.Controllers;

[ApiController]
[Route("[controller]")]
public class HomeController : ControllerBase
{
    private readonly UroContext _dbContext;
    private readonly HomeRepository _homeRepository;

    public HomeController(UroContext dbContext, HomeRepository homeRepository)
    {
        this._dbContext = dbContext;
        this._homeRepository = homeRepository;
    }

    [Authorize]
    [HttpGet("GetAuthenticatedUserHomes")]
    public IActionResult GetAuthenticatedUserHomes()
    {
        // Extracting person id from the token.
        var user = HttpContext.User;
        var UserIdClaim = user.FindFirst(c => c.Type == "PersonId")?.Value;

        if (UserIdClaim == null)
        {
            return BadRequest("Unable to authorize user.");
        }

        var userId = Convert.ToInt32(UserIdClaim);

        var homes = _homeRepository.GetUserHomes(userId);
        
        return Ok(homes);
    }
    
}