using CloudApi.Data;
using CloudApi.Repository;
using CommonData.Model.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudApi.Controllers;

/// <summary>
/// Provides controller functions related to the Home entity.
/// </summary>
[ApiController]
[Route("[controller]")]
public class HomeController : AbstractController
{
    private readonly UroContext _dbContext;
    private readonly HomeRepository _homeRepository;

    /// <summary>
    /// The constructor for the HomeController, dependencies should be injected by the framework.
    /// </summary>
    public HomeController(UroContext dbContext, HomeRepository homeRepository, PersonRepository personRepository) : base(personRepository)
    {
        this._dbContext = dbContext;
        this._homeRepository = homeRepository;
    }

    /// <summary>
    /// Get the homes that the currently authenticated user is a member of.
    /// </summary>
    [Authorize]
    [HttpGet("GetAuthenticatedUserHomes")]
    public ActionResult<IEnumerable<Home>> GetAuthenticatedUserHomes()
    {
        // Extracting person id from the token.
        var personId = GetAuthenticatedUserId();

        if (personId == null)
        {
            return BadRequest("Unable to authorize user.");
        }
        
        var homes = _homeRepository.GetUserHomes((int)personId);
        
        return Ok(homes);
    }



    /// <summary>
    /// Represents a DTO object for when creating homes.
    /// </summary>
    public record CreateHomeRequestDTO(string Name);
    
    /// <summary>
    /// Creates a home for the current authenticated user, and automatically puts the user inside it. 
    /// </summary>
    [Authorize]
    [HttpPost("CreateHome")]
    public ActionResult<Home> CreateHome([FromBody] CreateHomeRequestDTO dto)
    {

        // Grab the current authenticated user id from the token.
        var personId = GetAuthenticatedUserId();

        // If it cannot be found return a BadRequest.
        if (personId == null)
        {
            return BadRequest("Unable to authorize user.");
        }

        // Ghost person because this object is manually attached to the context.
        // This saves a roundtrip to the database.
        var ghostPerson = new Person() { Id = (int)personId };

        _dbContext.Persons.Attach(ghostPerson);
        
        var home = new Home
        {
            Name = dto.Name
        };
        
        // Add the current authenticated user (ghost object) to the home.
        home.Residents.Add(ghostPerson);

        // Mark the home for tracking by the ORM.
        var newHome = _dbContext.Homes.Add(home);

        // Save the changes.
        _dbContext.SaveChanges();

        // Return the created home object.
        return Ok(newHome.Entity);
    }
    
    
    
}