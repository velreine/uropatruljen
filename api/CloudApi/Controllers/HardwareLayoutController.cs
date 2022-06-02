using CloudApi.Repository;
using CommonData.Model.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudApi.Controllers;

/// <summary>
/// This controller provides endpoints for retrieving/manipulating HardwareLayouts.
/// </summary>
[ApiController]
[Route("[controller]")]
public class HardwareLayoutController : ControllerBase
{
    private readonly HardwareLayoutRepository _hardwareLayoutRepository;

    /// <summary>
    /// The constructor for the HardwareLayoutController, the dependencies is injected by the framework.
    /// </summary>
    public HardwareLayoutController(HardwareLayoutRepository hardwareLayoutRepository)
    {
        this._hardwareLayoutRepository = hardwareLayoutRepository;
    }

   
    /// <summary>
    /// Given an id gets a hardware layout.
    /// </summary>
    [Authorize]
    [HttpGet("GetLayout")]
    public ActionResult<HardwareLayout> GetLayout(int id)
    {
        var layout = _hardwareLayoutRepository.Find(id);

        if(layout == null) {
            return NotFound("The layout with the given id was not found.");
        }
        
        return layout;
    }
    
}