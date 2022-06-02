using CloudApi.Repository;
using CommonData.Model.Entity;
using Microsoft.AspNetCore.Mvc;

namespace CloudApi.Controllers;

/// <summary>
/// An abstract controller that provides convenience functions, eg. for dealing with security.
/// </summary>
public abstract class AbstractController : ControllerBase
{
    private readonly PersonRepository _personRepository;

    /// <summary>
    /// The constructor of the AbstractController.
    /// PersonRepository is a dependency for security related features.
    /// </summary>
    /// <param name="personRepository"></param>
    protected AbstractController(PersonRepository personRepository)
    {
        _personRepository = personRepository;
    }
    
    /// <summary>
    /// This function extracts the authenticated user id from the token,
    /// which means a roundtrip to the database does not happen.
    /// </summary>
    /// <returns>An int for the authenticated user, or null.</returns>
    [NonAction]
    protected int? GetAuthenticatedUserId()
    {
        // Extracting person id from the token.
        var user = HttpContext.User;
        var userIdClaim = user.FindFirst(c => c.Type == "PersonId")?.Value;

        if (userIdClaim == null)
        {
            return null;
        }

        return Convert.ToInt32(userIdClaim);
    }

    /// <summary>
    /// Uses the id from the token to find the user in the database,
    /// this means a database roundtrip occurs.
    /// </summary>
    /// <returns>The authenticated person if it exists.</returns>
    [NonAction]
    protected Person? GetAuthenticatedPerson()
    {
        var personId = GetAuthenticatedUserId();

        return personId == null 
            ? null
            : _personRepository.Find((int)personId)
            ;
    }
    
}