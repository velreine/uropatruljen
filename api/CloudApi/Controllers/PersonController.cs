using System.ComponentModel.DataAnnotations;
using CloudApi.Data;
using CommonData.Model.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CloudApi.Controllers;

/// <summary>
/// This controller provides endpoints for retrieving/manipulating Persons.
/// </summary>
[ApiController]
[Route("[controller]")]
public class PersonController : Controller
{

    private UroContext _dbContext;
    private IConfiguration _configuration;
    
    /// <summary>
    /// The constructor for the PersonController, the dependencies is injected by the framework.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="configuration">App configuration</param>
    public PersonController(UroContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _configuration = configuration;
    }
}