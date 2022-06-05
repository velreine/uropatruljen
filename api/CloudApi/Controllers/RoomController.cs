using CloudApi.Data;
using CommonData.Model.DTO;
using CommonData.Model.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudApi.Controllers;

/// <summary>
/// This controller provides endpoints for retrieving/manipulating Persons.
/// </summary>
[ApiController]
[Route("[controller]")]
public class RoomController : ControllerBase
{
    private readonly UroContext _dbContext;

    /// <summary>
    /// The constructor for the RoomController, the dependencies is injected by the framework.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public RoomController(UroContext dbContext)
    {
        _dbContext = dbContext;
    }


    /// <summary>
    /// Creates a room for the current authenticated user. 
    /// </summary>
    [Authorize]
    [HttpPost("CreateRoom")]
    public ActionResult<CreateRoomResponseDTO> CreateRoom([FromBody] CreateRoomRequestDTO dto)
    {

        // TODO: Ensure that the Person is a member of the Home that the room is being created in.
        
        // Construct the room from the dto.
        var room = new Room(null, dto.Name, dto.HomeId);

        // Add the room to the context.
        var newRoom = _dbContext.Rooms.Add(room).Entity;

        // Flush the Unit of Work.
        _dbContext.SaveChanges();
        
        // Create Response DTO.
        var responseData = new CreateRoomResponseDTO((int)newRoom.Id!, newRoom.Name, newRoom.HomeId);

        // Return the response.
        return Ok(responseData);
    }
    
    
    
    
    
    
}