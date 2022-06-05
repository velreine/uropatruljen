using CloudApi.Data;
using CloudApi.Repository;
using CommonData.Model.DTO;
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
        _dbContext = dbContext;
        _homeRepository = homeRepository;
    }

    /// <summary>
    /// Get the homes that the currently authenticated user is a member of.
    /// </summary>
    [Authorize]
    [HttpGet("GetAuthenticatedUserHomes")]
    public ActionResult<IEnumerable<AuthenticatedUserHomeResponseDTO>> GetAuthenticatedUserHomes()
    {
        // Extracting person id from the token.
        var personId = GetAuthenticatedUserId();

        if (personId == null)
        {
            return BadRequest("Unable to authorize user.");
        }
        
        // Get all Home Entities.
        var homes = _homeRepository.GetUserHomes((int)personId);

        // Map/Transform the homes.
        var responseData = homes.Select(homeEntity =>
        {
            // Map Room entities to type AuthenticatedUserRoom.
            var rooms = homeEntity.Rooms.Select(roomEntity =>
                new AuthenticatedUserRoom((int)roomEntity.Id!, roomEntity.Name, roomEntity.HomeId));

            // Map Home entities to type AuthenticatedUserHomeResponseDTO
            return new AuthenticatedUserHomeResponseDTO((int)homeEntity.Id!, homeEntity.Name, rooms.ToList());
        }).ToList();
        
        // Return the data.
        return Ok(responseData);
    }
    
    /// <summary>
    /// Creates a home for the current authenticated user, and automatically puts the user inside it. 
    /// </summary>
    [Authorize]
    [HttpPost("CreateHome")]
    public ActionResult<CreateHomeResponseDTO> CreateHome([FromBody] CreateHomeRequestDTO dto)
    {
        
        // Grab the current authenticated user id from the token.
        var authenticatedPerson = GetAuthenticatedPerson();

        // If it cannot be found return a BadRequest.
        if (authenticatedPerson == null)
        {
            return BadRequest("Unable to authorize user.");
        }
        
        var home = new Home(null, dto.Name);
        
        // Add the current authenticated user (ghost object) to the home.
        home.AddPerson(authenticatedPerson);

        // Mark the home for tracking by the ORM.
        var newHome = _dbContext.Homes.Add(home).Entity;

        // Map to DTO.
        var responseData = new CreateHomeResponseDTO((int)newHome.Id!, newHome.Name);
        
        // Save the changes.
        _dbContext.SaveChanges();

        // Return the created home object.
        return Ok(responseData);
    }
    
    
    
}