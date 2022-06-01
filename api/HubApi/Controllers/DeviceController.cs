using System.Diagnostics.CodeAnalysis;
using HubApi.Settings;
using Microsoft.AspNetCore.Mvc;

namespace HubApi.Controllers;

[ApiController]
[Route("[controller]")]
public class DeviceController : ControllerBase
{
    private readonly MqttAppSettings _mqttSettings;
    private readonly HardwareSettings _hardwareSettings;
    public record DeviceInformationResponseDTO(string MqttEndpoint, string SerialNumber, string ModelNumber);

    public DeviceController(MqttAppSettings mqttSettings, HardwareSettings hardwareSettings)
    {
        _mqttSettings = mqttSettings;
        _hardwareSettings = hardwareSettings;
    }
    
    /// <summary>
    /// Returns information about the device the api is connected to.
    /// </summary>
    [HttpGet("/information")]
    [SuppressMessage("ReSharper", "RedundantAnonymousTypePropertyName")]
    public ActionResult<DeviceInformationResponseDTO> GetDeviceInformation()
    {
        return Ok(new DeviceInformationResponseDTO(
            _mqttSettings.Endpoint,
            _hardwareSettings.SerialNumber,
            _hardwareSettings.ModelNumber
            ));
    }
    
}