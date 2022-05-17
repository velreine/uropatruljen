using Microsoft.AspNetCore.Mvc;

namespace HubApi.Controllers;

[ApiController]
[Route("[controller]")]
public class DevelopmentController
{

    public DevelopmentController()
    {
        
    }

    public class DevelopmentRequestDTO
    {
        public string Echo { get; set; } // Echo => echo
    }
    
    [HttpPost(Name = "DevEndpoint")]
    public ActionResult<object> Post(DevelopmentRequestDTO request)
    {

        var response = new ActionResult<object>(new 
        {
            Echo = request.Echo,
        });

        return response;
    }
    
}