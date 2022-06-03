using CommonData.Logic.Factory;
using CommonData.Model.Action;
using HubApi.Handler;


namespace HubApi.Logic;

/// <summary>
/// Represents our default Action Handlers.
/// </summary>
public class DefaultActionHandlers : IActionHandler, IDefaultActionHandlers
{
    private readonly IEnumerable<IActionHandler<IAction>> actionHandlers;
    private readonly Dictionary<Type, Action<IAction>> _handlers;

    /// <summary>
    /// Instantiate Default Action handlers for the Hub Api.
    /// </summary>
    public DefaultActionHandlers(IEnumerable<IActionHandler<IAction>> actionHandlers)
    {
        this.actionHandlers = actionHandlers;
        _handlers = new Dictionary<Type, Action<IAction>>()
        {
            {typeof(SetColorAction), HandleSetColorAction},
            {typeof(TurnOnOffAction), HandleTurnOnOffAction},
        };
    }

    private void HandleSetColorAction(IAction action)
    {
        new SetColorActionHandler().HandleAsync((SetColorAction)action);
    }

    private void HandleTurnOnOffAction(IAction action)
    {
        new TurnOnOffActionHandler().HandleAsync((TurnOnOffAction)action);
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public async Task AssignToHandler(IAction action)
    {
        switch (action)
        {
            case SetColorAction:
                IActionHandler<IAction>? setColor = null;
                foreach (var ah in actionHandlers)
                {
                    if ((Type)ah == typeof(IActionHandler<SetColorAction>))
                    {
                        setColor = ah;
                        await setColor.HandleAsync(action);
                        break;
                    }
                }
                break;
            case TurnOnOffAction:
                IActionHandler<IAction>? onOff = null;
                foreach (var ah in actionHandlers)
                {
                    if ((Type)ah == typeof(IActionHandler<TurnOnOffAction>))
                    {
                        onOff = ah;
                        await onOff.HandleAsync(action);
                        break;
                    }
                }
                
                break;
        }
        // foreach (var handler in actionHandlers)
        // {
        //     await handler.HandleAsync(action);
        // }
    }
}