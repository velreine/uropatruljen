using HubApi.Model.Entity;
using Microsoft.AspNetCore.Mvc;
using HubApi.Boards;

namespace HubApi.Controllers;

[ApiController]
[Route("[controller]")]
public class BoardController
{
    
    [HttpGet(Name = "GetBoardConfiguration")]
    public ActionResult<HardwareConfiguration> GetBoardConfiguration(int id)
    {
        // TODO: Actually look up the board by id.
        var board = Board.SmartUroV1;
        
        var response = new ActionResult<HardwareConfiguration>(board);

        return response;
    }
    
}