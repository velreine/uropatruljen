using CloudApi.Data;
using CommonData.Model.Entity;
using Microsoft.AspNetCore.Mvc;
using MQTTnet.Server;

namespace CloudApi.Controllers;


[Route("[controller]")]
[ApiController]
public class DevelopmentController : ControllerBase
{


    private readonly UroContext _dbContext;
    private readonly MqttServer _mqttServer;
    
    public DevelopmentController(UroContext dbContext, MqttServer mqttServer)
    {
        this._dbContext = dbContext;
        this._mqttServer = mqttServer;
    }
    
    [HttpGet(Name = "DoStuff")]
    public IActionResult DoStuff()
    {

        // TODO: TEST MQTT SERVER IS INJECTED PROPERLY.
        
        /*var state = new RgbComponentState
        {
            Component = null,
            Device = null,
            IsOn = true,
            RValue = 255,
            GValue = 100,
            BValue = 50
        };

        _dbContext.ComponentStates.Add(state);

        _dbContext.SaveChanges();*/
        
        
        return Ok("OK");
    }
    
}