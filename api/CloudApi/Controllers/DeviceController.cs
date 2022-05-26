using CommonData.Model.Entity;
using Microsoft.AspNetCore.Mvc;

namespace CloudApi.Controllers;

public class DeviceController : ControllerBase
{

    [HttpGet(Name = "GetDevices")]
    public IEnumerable<Device> Get()
    {
        var devices = new List<Device>();

        return devices;
    }
    
}