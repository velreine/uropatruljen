using Microsoft.AspNetCore.Mvc;

namespace HubApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ComponentController
{
    
    [HttpPost(Name = "SetComponentState")]
    public ActionResult<object> SetComponentState(int componentId)
    {
        
        
        //var response = new ActionResult<HardwareConfiguration>(board);

        //return response;
    }
    
    
    
}