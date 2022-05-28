using CloudApi.Data;
using CommonData.Model.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace CloudApi.Controllers;

[ApiController]
[Route("[controller]")]
public class HardwareLayoutController : ControllerBase
{
    private readonly UroContext _dbContext;

    public HardwareLayoutController(UroContext dbContext)
    {
        this._dbContext = dbContext;
    }

   
    [Authorize]
    [HttpGet("GetLayout")]
    public ActionResult<HardwareLayout> Get(int id)
    {
        var layout = _dbContext.HardwareLayouts.Find(id);

        if(layout == null) {
            return NotFound("The layout with the given id was not found.");
        }
        
        return layout;
    }
    
}