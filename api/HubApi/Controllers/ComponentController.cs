using Microsoft.AspNetCore.Mvc;

namespace HubApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ComponentController : ControllerBase
{
    
    [HttpPost(Name = "SetComponentState")]
    public IActionResult SetComponentState(int componentId)
    {
        
        
        //var response = new ActionResult<HardwareConfiguration>(board);

        //return response;

        return this.Ok(new
        {
            Foo = "Bar"
        });

    }
    
    
    
}