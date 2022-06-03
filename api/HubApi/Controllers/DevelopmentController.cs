using System.Device.Gpio;
using CommonData.Model.Action;
using Microsoft.AspNetCore.Mvc;
using MQTTnet;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
#pragma warning disable CS8618
#pragma warning disable CS1591

namespace HubApi.Controllers;

/// <summary>
/// This controller is mainly used in Development for debugging and stuff.
/// </summary>
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
    
    /// <summary>
    /// Debugging endpoint uses for multiple purposes while developing.
    /// </summary>
    [HttpPost(Name = "DevEndpoint")]
    public ActionResult<object> Post(DevelopmentRequestDTO request)
    {

        // GPIO 17 which is physical pin 11.
        int ledPin1 = 17;
        GpioController controller = new();
        // Set the pin to output mode so we can switch something on.
        controller.OpenPin(ledPin1, PinMode.Output);

        controller.Write(ledPin1, PinValue.Low);
        PinValue value = controller.Read(ledPin1);
        
        var response = new ActionResult<object>(new 
        {
            Echo = request.Echo,
        });

        return response;
    }
}