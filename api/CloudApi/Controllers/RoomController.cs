using CloudApi.Data;
using CloudApi.Repository;
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
public class RoomController : AbstractController
{
    private readonly UroContext _dbContext;
    private readonly RoomRepository _roomRepository;

    /// <summary>
    /// The constructor for the RoomController, the dependencies is injected by the framework.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="roomRepository">The room repository</param>
    /// <param name="personRepository">The person repository</param>
    public RoomController(
        UroContext dbContext,
        RoomRepository roomRepository,
        PersonRepository personRepository) : base(personRepository)
    {
        _dbContext = dbContext;
        _roomRepository = roomRepository;
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

    /// <summary>
    /// Deletes a room with the given id.
    /// </summary>
    /// <param name="id">The id of the room to delete.</param>
    [Authorize]
    [HttpDelete("DeleteRoom")]
    public ActionResult DeleteRoom(int id)
    {
        // Grab the current authenticated user id from the token.
        var authenticatedPerson = GetAuthenticatedPerson();
        
        // If it cannot be found return a BadRequest.
        if (authenticatedPerson == null)
        {
            return BadRequest("Unable to authorize user.");
        }
        
        // Find the room in the DB.
        var deletedRoom = _roomRepository.Delete(id);

        // Null means the room could not be deleted, for some reason.
        if (deletedRoom == null)
        {
            return BadRequest("The room could not be deleted.");
        }

        // If we get this far, all is good.
        return Ok("The room was deleted.");
    }
    
    /// <summary>
    /// Updates the room.
    /// </summary>
    [Authorize]
    [HttpPut("UpdateRoom")]
    public ActionResult<UpdateRoomResponseDTO> UpdateRoom([FromBody] UpdateRoomRequestDTO dto)
    {
        // Grab the current authenticated user id from the token.
        var authenticatedPerson = GetAuthenticatedPerson();
        
        // If it cannot be found return a BadRequest.
        if (authenticatedPerson == null)
        {
            return BadRequest("Unable to authorize user.");
        }
        
        // Fetch room from database.
        var room = _roomRepository.Find(dto.Id);

        // If the entity was not found return 404.
        if (room == null)
        {
            return NotFound("The room could not be found.");
        }
        
        // Manipulate the Entity model according to the DTO.
        room.Name = dto.Name;
        
        // Actually update the entity.
        var updatedRoom = _roomRepository.Update(room);
        
        // Map to DTO.
        var responseData = new UpdateRoomResponseDTO((int)updatedRoom.Id!, updatedRoom.Name);
        
        // Return the dto.
        return Ok(responseData);
    }
    
    
    
    
}