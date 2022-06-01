using CloudApi.Controllers;
using CloudApi.Data;
using CommonData.Model.Entity;

namespace CloudApi.Repository;

/// <summary>
/// Provides different convenience functions for retrieving Person entities based on different criteria.
/// </summary>
public class PersonRepository
{
    private readonly UroContext _dbContext;

    /// <summary>
    /// The constructor for the PersonRepository, the database context is injected by the framework.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public PersonRepository(UroContext dbContext)
    {
        this._dbContext = dbContext;
    }

    /// <summary>
    /// Given a Person id finds the person by that id.
    /// </summary>
    /// <param name="id">Id of the person to find,</param>
    /// <returns>A Person entity if found.</returns>
    public Person? Find(int id)
    {
        return _dbContext.Persons.Find(id);
    } 
}