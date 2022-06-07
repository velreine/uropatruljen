using CloudApi.Data;
using CommonData.Model.Entity;
using Microsoft.EntityFrameworkCore;

namespace CloudApi.Repository;

/// <summary>
/// Provides convenience functions for retrieving/manipulating Rooms based on different criteria.
/// </summary>
public class RoomRepository
{
    private readonly UroContext _dbContext;

    /// <summary>
    /// The constructor for the RoomRepository, the database context is injected by the framework.
    /// </summary>
    /// <param name="dbContext"></param>
    public RoomRepository(UroContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    /// <summary>
    /// Given a Room id finds the room by that id.
    /// </summary>
    /// <param name="id">Id of the room to find,</param>
    /// <returns>A Room entity if found.</returns>
    public Room? Find(int id)
    {
        return _dbContext.Rooms.Find(id);
    }

    /// <summary>
    /// Updates a room.
    /// </summary>
    /// <param name="room"></param>
    /// <returns></returns>
    public Room Update(Room room)
    {
        _dbContext.Entry(room).State = EntityState.Modified;
        _dbContext.SaveChanges();
        return room;
    }

    /// <summary>
    /// Given an id, deletes a room.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Room? Delete(int id)
    {
        var room = _dbContext.Rooms.Find(id);
        
        if (room == null)
        {
            return room;
        }

        _dbContext.Rooms.Remove(room);
        _dbContext.SaveChanges();
        
        return room;
    }
}