using System.Diagnostics.CodeAnalysis;
using HubApi.Settings;
using Microsoft.AspNetCore.Mvc;

namespace HubApi.Controllers;

/// <summary>
/// This controller provides convenience endpoints about the smart device itself.
/// </summary>
[ApiController]
[Route("[controller]")]
public class DeviceController : ControllerBase
{
    private readonly AppSettings _appSettings;


    /// <inheritdoc />
    public DeviceController(AppSettings appSettings)
    {
        _appSettings = appSettings;
    }
    
    /// <summary>
    /// Returns the App Settings that the Api is configured with.
    /// </summary>
    [HttpGet("/information")]
    [SuppressMessage("ReSharper", "RedundantAnonymousTypePropertyName")]
    public ActionResult<AppSettings> GetDeviceInformation()
    {
        return Ok(_appSettings);
    }
    
}