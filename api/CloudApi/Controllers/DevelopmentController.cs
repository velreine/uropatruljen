using CloudApi.Data;
using Microsoft.AspNetCore.Mvc;
using MQTTnet.Server;

namespace CloudApi.Controllers;


/// <summary>
/// This controller hosts various endpoints that are used for testing while developing.
/// </summary>
[Route("[controller]")]
[ApiController]
public class DevelopmentController : ControllerBase
{


    private readonly UroContext _dbContext;
    private readonly MqttServer _mqttServer;
    
    /// <summary>
    /// The constructor for the DevelopmentController, dependencies are resolved and injected by the framework.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="mqttServer"></param>
    public DevelopmentController(UroContext dbContext, MqttServer mqttServer)
    {
        this._dbContext = dbContext;
        this._mqttServer = mqttServer;
    }
    
    /// <summary>
    /// Does unpredictable stuff as this is a controller used for testing various things.
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "DoStuff")]
    public IActionResult DoStuff()
    {

        // TODO: TEST MQTT SERVER IS INJECTED PROPERLY.
        
        
        
        //_mqttServer.InjectApplicationMessage()
        
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