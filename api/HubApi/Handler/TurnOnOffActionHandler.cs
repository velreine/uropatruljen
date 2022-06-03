using CommonData.Logic.Factory;
using CommonData.Model.Action;
using HubApi.Manager;

namespace HubApi.Handler;

/// <summary>
/// 
/// </summary>
public class TurnOnOffActionHandler : IActionHandler<TurnOnOffAction>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public Task HandleAsync(TurnOnOffAction action)
    {
        var componentIsOn = ComponentManager.ComponentIsOn(action.ComponentIdentifier);
        
        if (action.TurnOn && !componentIsOn)
            ComponentManager.TurnOn(action.ComponentIdentifier);
        else if (!action.TurnOn && componentIsOn)
            ComponentManager.TurnOff(action.ComponentIdentifier);
        
        return Task.CompletedTask;
    }
}