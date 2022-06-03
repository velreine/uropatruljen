using CommonData.Logic.Factory;
using CommonData.Model.Action;
using HubApi.Manager;

namespace HubApi.Handler;

/// <summary>
/// An action handler for handling requests to change RGB values of a component. 
/// </summary>
public class SetColorActionHandler : IActionHandler<SetColorAction>
{

    /// <summary>
    /// Handles a action / request to change the RGB values.
    /// </summary>
    /// <param name="action">Set color action</param>
    /// <returns>Task.CompletedTask</returns>
    /// <exception cref="Exception"></exception>
    public Task HandleAsync(SetColorAction action)
    {
        ComponentManager.SetColorComponent
            (
                action.ComponentIdentifier, 
                action.RValue, 
                action.GValue, 
                action.BValue
            );

        return Task.CompletedTask;
    }
}