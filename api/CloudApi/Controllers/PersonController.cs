using System.ComponentModel.DataAnnotations;
using CloudApi.Data;
using CommonData.Model.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CloudApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PersonController : Controller
{

    private UroContext _dbContext;
    private IConfiguration _configuration;
    
    public PersonController(UroContext dbContext, IConfiguration configuration)
    {
        this._dbContext = dbContext;
        this._configuration = configuration;
    }
    
    
    [Authorize]
    [HttpPost("RegisterPerson")]
    public IActionResult Register([FromBody] RegisterPersonRequestDTO dto)
    {

        
        
        // Check to see if a person already exists with the given e-mail.
        var existingUser = _dbContext.Persons.FirstOrDefault(p => p.Email == dto.Email);

        // If one such user exists, return a bad request informing the client.
        if (existingUser != null)
        {
            return BadRequest("There is already another user with that e-mail in the system.");
        }

        /*var personToAdd = new Person()
        {
            Email = dto.Email,
            Name = dto.Name,
            
        }*/
        
        //_dbContext.Persons.Add
        
        return new ObjectResult(new
        {
            JwtKey = _configuration["Jwt:Key"],
            JwtIssuer = _configuration["Jwt:Issuer"],
            dto.Name,
            dto.Email,
            dto.PlainTextPassword,
            Message = "DEBUG..."
        });

    }

    public class RegisterPersonRequestDTO
    {
        [Required]
        [MaxLength(32)]
        public string? Name { get; set; }
        
        [Required]
        [MaxLength(255)]
        public string? Email { get; set; }
        
        [Required]
        [MaxLength(32)]
        public string? PlainTextPassword { get; set; }
    }
    
}