using CommonData.Model.Entity;
using CommonData.Model.Static;
using Microsoft.AspNetCore.Mvc;


namespace HubApi.Controllers;

[ApiController]
[Route("[controller]")]
public class BoardController
{
    
    [HttpGet(Name = "GetBoardConfiguration")]
    public ActionResult<HardwareLayout> GetBoardConfiguration(int id)
    {
        // TODO: Actually look up the board by id.
        var board = Board.SmartUroV1;
        
        // Figure out if we should render light controls.
        if (board.AttachedComponents.Any(comp => comp.Type == ComponentType.Diode))
        {
            
        }
        
        
        var response = new ActionResult<HardwareLayout>(board);

        return response;
    }
    
}