using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CloudApi.Data;
using CloudApi.Repository;
using CommonData.Model.DTO;
using CommonData.Model.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CloudApi.Controllers;

/// <summary>
/// This controller provides endpoints related to security,authorization and authentication.
/// </summary>
[Route("[controller]")]
[ApiController]
public class AuthenticationController : AbstractController
{
    private readonly IConfiguration _config;
    private readonly UroContext _dbContext;
    private readonly IPasswordHasher<Person> _hasher;
    
    /// <summary>
    /// The constructor for the AuthenticationController, the dependencies is resolved and injected by the framework.
    /// </summary>
    /// <param name="config">Configuration settings</param>
    /// <param name="dbContext">Database context.</param>
    /// <param name="hasher">A service that can hash passwords.</param>
    /// <param name="personRepository">Person repository security.</param>
    public AuthenticationController(IConfiguration config, UroContext dbContext, IPasswordHasher<Person> hasher, PersonRepository personRepository) : base(personRepository)
    {
        _config = config;
        _dbContext = dbContext;
        _hasher = hasher;
    }

    /// <summary>
    /// The Cloud Api's authentication endpoint for registered users/persons.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("/auth")]
    public ActionResult<LoginResponseDTO> Login([FromBody] LoginRequestDTO dto)
    {
        const string failedMsg = "Login failed the password or e-mail was wrong.";

        // Lookup the user in the database.
        var user = _dbContext.Persons.FirstOrDefault(person => person.Email == dto.Email);

        // If the user was found return a generic message to the client, (to prevent e-mail snooping)
        if (user == null)
        {
            return Unauthorized(failedMsg);
        }

        var passwordResult = _hasher.VerifyHashedPassword(user, user.HashedPassword, dto.Password);

        // If the password is valid, create a token for the user.
        if (passwordResult == PasswordVerificationResult.Success)
        {
            var token = GenerateJsonWebToken(user);
            return Ok(new LoginResponseDTO("OK", token));
        }


        return Unauthorized(failedMsg);
    }

    /// <summary>
    /// An endpoint that allows persons to register themselves as users of our app.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("/register")]
    public ActionResult<RegisterResponseDTO> Register([FromBody] RegisterRequestDTO dto)
    {
        const string userAlreadyExistsMsg = "It was not possible to register, the e-mail is taken.";
        
        // Check if this e-mail is already registered.
        var existingUser = _dbContext.Persons.FirstOrDefault(person => person.Email == dto.Email);

        if (existingUser != null)
        {
            return BadRequest(userAlreadyExistsMsg);
        }
        
        var newUser = new Person(dto.Name!, dto.Email!);
        newUser.HashedPassword = _hasher.HashPassword(newUser, dto.Password);

        var createdUser = _dbContext.Persons.Add(newUser);
        _dbContext.SaveChanges();

        return Ok(new RegisterResponseDTO("OK, user created!", newUser.Id));
    }

    /// <summary>
    /// Returns the current authenticated user (based on the JWT Bearer Token).
    /// </summary>
    [Authorize]
    [HttpGet("/WhoAmI")]
    public ActionResult<Person> WhoAmI()
    {
        // Get the currently authenticated user.
        var person = GetAuthenticatedPerson();
        
        if (person == null)
        {
            return BadRequest("Unable to authorize user.");
        }

        // Conceal the hashed password from the client.
        person.HashedPassword = null;
        
        // Return the found object.
        return Ok(person);
    }
    
    [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
    private string GenerateJsonWebToken(Person person)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? throw new InvalidOperationException()));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // claims: https://datatracker.ietf.org/doc/html/rfc7519#section-4
        var claims = new Claim[]
        {
            // Sub uniquely identifies the subject (which is a person).
            new Claim("PersonId", person.Id.ToString())
        };

        var token = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Issuer"],
            claims,
            expires: DateTime.Now.AddDays(31),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}