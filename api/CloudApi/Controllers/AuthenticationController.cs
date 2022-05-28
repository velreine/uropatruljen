using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CloudApi.Data;
using CloudApi.Repository;
using CommonData.Model.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CloudApi.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthenticationController : Controller
{
    private readonly IConfiguration _config;
    private readonly UroContext _dbContext;
    private readonly IPasswordHasher<Person> _hasher;
    private readonly PersonRepository _personRepository;

    public AuthenticationController(IConfiguration config, UroContext dbContext, IPasswordHasher<Person> hasher, PersonRepository personRepository)
    {
        _config = config;
        _dbContext = dbContext;
        _hasher = hasher;
        _personRepository = personRepository;
    }

    [AllowAnonymous]
    [HttpPost("/auth")]
    public IActionResult Login([FromBody] LoginRequestDTO dto)
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
            return Ok(new { token });
        }


        return Unauthorized(failedMsg);
    }

    [AllowAnonymous]
    [HttpPost("/register")]
    public IActionResult Register([FromBody] RegisterRequestDTO dto)
    {
        const string userAlreadyExistsMsg = "It was not possible to register, the e-mail is taken.";
        
        // Check if this e-mail is already registered.
        var existingUser = _dbContext.Persons.FirstOrDefault(person => person.Email == dto.Email);

        if (existingUser != null)
        {
            return BadRequest(userAlreadyExistsMsg);
        }

        var newUser = new Person
        {
            Name = dto.Name,
            Email = dto.Email
        };
        
        newUser.HashedPassword = _hasher.HashPassword(newUser, dto.Password);

        var createdUser = _dbContext.Persons.Add(newUser);
        _dbContext.SaveChanges();

        return Ok(new
        {
            Message = "OK - User created.",
            Id = createdUser.Entity.Id,
        });

    }

    [Authorize]
    [HttpGet("/WhoAmI")]
    public IActionResult WhoAmI()
    {
        // Extracting person id from the jwt.
        var user = HttpContext.User;
        var UserIdClaim = user.FindFirst(c => c.Type == "PersonId")?.Value;

        if (UserIdClaim == null)
        {
            return BadRequest("Unable to authorize user.");
        }

        var userId = Convert.ToInt32(UserIdClaim);

        // Find the person, with the authenticated user id.
        var person = _personRepository.Find(userId);

        // Do not leak even the hashed password.
        if (person != null)
        {
            person.HashedPassword = null;
        }
        
        // Return the found object.
        return Ok(person);
    }


    public class RegisterRequestDTO
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
    
    public class LoginRequestDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }
    }

    private string GenerateJsonWebToken(Person person)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
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