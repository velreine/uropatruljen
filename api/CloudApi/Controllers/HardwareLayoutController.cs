using CloudApi.Data;
using CloudApi.Repository;
using CommonData.Model.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CloudApi.Controllers;

[ApiController]
[Route("[controller]")]
public class HardwareLayoutController : ControllerBase
{
    private readonly HardwareLayoutRepository _hardwareLayoutRepository;

    public HardwareLayoutController(HardwareLayoutRepository hardwareLayoutRepository)
    {
        this._hardwareLayoutRepository = hardwareLayoutRepository;
    }

   
    [Authorize]
    [HttpGet("GetLayout")]
    public ActionResult<HardwareLayout> Get(int id)
    {
        var layout = _hardwareLayoutRepository.Find(id);

        if(layout == null) {
            return NotFound("The layout with the given id was not found.");
        }
        
        return layout;
    }
    
}