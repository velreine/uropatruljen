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
    
}