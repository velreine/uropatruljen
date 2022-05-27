using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using CommonData.Model.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudApi.Controllers;

[ApiController]
[Route("[controller]")]
public class DeviceController : ControllerBase
{

    /*[HttpGet(Name = "GetDevices")]
    public IEnumerable<Device> Get()
    {

        //var sampleDev = new Device();
        //sampleDev.Id = 1;
        //sampleDev.Name = "1";
        //sampleDev.Layout = 1; // instantiate layout.
        
        
        
        
        var devices = new List<Device>();

        return devices;
    }*/

    [Authorize]
    [HttpGet(Name = "GetDevice")]
    //[Route("[controller]/{id:int}")]
    // POST /api/polices
    // PUT /api/policy/1009
    public Device GetOne([FromRoute] int id)
    {

        // Extracting person id from the token.
        var user = HttpContext.User;
        var UserId = user.FindFirst(c => c.Type == "PersonId")?.Value;

        var allClaims = user.Claims;

        return new Device() { SerialNumber = "USER ID IS " + UserId};
        
        using var conn = new SqlConnection("Server=localhost;Database=uro_db;User Id=sa;Password=12345");
        conn.Open();

        var query = $"SELECT id, name, serial_number, hardware_layout_id, room_id FROM device WHERE id = {id};";

        var cmd = new SqlCommand(query, conn);

        var device = new Device();
        var hwl = new HardwareLayout();
        var room = new Room();

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            device.Id = reader.GetInt32(0);
            device.Name = reader.GetString(1);
            device.SerialNumber = reader.GetString(2);
            hwl.Id = reader.GetInt32(3);
            room.Id = reader.GetInt32(4);

            device.Layout = hwl;
            device.Room = room;
        }

        return device;
    }
    
}