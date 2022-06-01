using CommonData.Logic.Factory;
using CommonData.Model.Action;



namespace HubApi.Logic;

/// <summary>
/// Represents our default Action Handlers.
/// </summary>
public class DefaultActionHandlers : IActionHandler
{
    private readonly Dictionary<Type, Action<IAction>> _handlers;

    /// <summary>
    /// Instantiate Default Action handlers for the Hub Api.
    /// </summary>
    public DefaultActionHandlers()
    {
        _handlers = new Dictionary<Type, Action<IAction>>()
        {
            {typeof(SetColorAction), HandleSetColorAction},
            {typeof(TurnOnOffAction), HandleTurnOnOffAction},
        };
    }

    private void HandleSetColorAction(IAction obj)
    {
        throw new NotImplementedException();
    }

    private void HandleTurnOnOffAction(IAction obj)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Method responsible for handling an Action.
    /// In the context of the class "DefaultActionHandlers", this method simply finds the correct handler for the
    /// Action Type and invokes it, and returns the result of that.
    /// </summary>
    /// <param name="action">The action that should be handled.</param>
    /// <exception cref="Exception">If no handler could be found for the action type.</exception>
    public Task HandleAsync(IAction action)
    {
        // Fetch the handler from the map.
        _handlers.TryGetValue(action.GetType(), out var actionHandler);

        if (actionHandler == null)
        {
            throw new Exception($"No handler could be found for the Action of type: {action.GetType()}");
        }

        // Invoke the correct handler.
        actionHandler.Invoke(action);

        return Task.CompletedTask;
    }
}