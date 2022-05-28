using CloudApi.Data;
using CommonData.Model.Entity;
using Microsoft.AspNetCore.Mvc;

namespace CloudApi.Controllers;


[Route("[controller]")]
[ApiController]
public class DevelopmentController : ControllerBase
{


    private readonly UroContext _dbContext;
    
    public DevelopmentController(UroContext dbContext)
    {
        this._dbContext = dbContext;
    }
    
    [HttpGet(Name = "DoStuff")]
    public IActionResult DoStuff()
    {

        var state = new RgbComponentState
        {
            Component = null,
            Device = null,
            IsOn = true,
            RValue = 255,
            GValue = 100,
            BValue = 50
        };

        _dbContext.ComponentStates.Add(state);

        _dbContext.SaveChanges();
        
        
        return Ok("OK");
    }
    
}