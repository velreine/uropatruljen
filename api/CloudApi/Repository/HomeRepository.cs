using CloudApi.Data;
using CommonData.Model.Entity;
using Microsoft.EntityFrameworkCore;

namespace CloudApi.Repository;

/// <summary>
/// Provides convenience functions for retrieving Homes based on different criteria.
/// </summary>
public class HomeRepository
{
    private readonly UroContext _dbContext;
    
    /// <summary>
    /// The constructor for the HomeRepository, the database context is injected by the framework.
    /// </summary>
    public HomeRepository(UroContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Given a user/person id retrieves the homes that the person is a member of.
    /// </summary>
    /// <param name="userId">The id of the user.</param>
    /// <returns>The homes the user has access to.</returns>
    public IEnumerable<Home> GetUserHomes(int userId)
    {
        var homes = 
            _dbContext
                .Homes
                .Where(home => home.Persons.Any(p => p.Id == userId))
                .Include(home => home.Rooms)
            ;

        return homes;
    }

    /// <summary>
    /// Given a Home id finds the home by that id.
    /// </summary>
    /// <param name="id">Id of the home to find,</param>
    /// <returns>A Home entity if found.</returns>
    public Home? Find(int id)
    {
        return _dbContext.Homes.Find(id);
    }

    public Home Update(Home home)
    {
        _dbContext.Entry(home).State = EntityState.Modified;
        _dbContext.SaveChanges();
        return home;
    }

    public Home? Delete(int id)
    {
        var home = _dbContext.Homes.Find(id);
        
        if (home == null)
        {
            return home;
        }

        _dbContext.Homes.Remove(home);
        _dbContext.SaveChanges();
        
        return home;
    }
}