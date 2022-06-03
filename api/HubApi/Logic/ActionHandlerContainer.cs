using CommonData.Logic.Factory;
using CommonData.Model.Action;
using HubApi.Handler;


namespace HubApi.Logic;

/// <summary>
/// Container for different IActionHandler services.
/// Delegates handle to the proper handler.
/// </summary>
public class ActionHandlerContainer : IActionHandler
{
    private readonly Dictionary<Type, IActionHandler> _handlers = new();

    /// <summary>
    /// Instantiate the container.
    /// </summary>
    public ActionHandlerContainer()
    {
    }

    /// <summary>
    /// Provides a runtime handler registrator, useful when doing automatic DI-registration.
    /// </summary>
    /// <param name="actionType">The runtime type of the action.</param>
    /// <param name="handler">The handler for the action.</param>
    /// <returns>The ActionHandlerContainer (fluently)</returns>
    /// <exception cref="Exception">Throws an exception if the passed type is not of type IAction, or if a handler method is already registered for the action type.</exception>
    public ActionHandlerContainer RegisterHandler(Type actionType, IActionHandler handler)
    {
        
            if (actionType.GetType() != typeof(IAction))
            {
                throw new Exception(
                    $"Cannot register action handler for type: {actionType} that does not implement IAction.");
            }
            
            if (_handlers.ContainsKey(actionType))
            {
                throw new Exception(
                    $"The handler container already contains an ActionHandler for the type: {actionType}");
            }
            
            _handlers.Add(actionType, handler);

            // Fluent interface.
            return this;
    }

    /// <summary>
    /// Compile-time generic handler registrator, useful when doing manual DI-registration.
    /// </summary>
    /// <param name="handler">The handler that should handle the action of type T.</param>
    /// <typeparam name="T">The type of the action.</typeparam>
    /// <returns>The ActionHandlerContainer back (fluently)</returns>
    /// <exception cref="Exception">If a handler is already registered for the type.</exception>
    public ActionHandlerContainer RegisterHandler<T>(IActionHandler<T> handler) where T : IAction, new()
    {
        if (_handlers.ContainsKey(typeof(T)))
        {
            throw new Exception(
                $"The handler container already contains an ActionHandler for the type: {typeof(T)}");
        }
        
        _handlers.Add(typeof(T), handler);
        
        // Fluent interface
        return this;
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

        // Fetch the correct handler based on the runtime type of the action instance.
        _handlers.TryGetValue(action.GetType(), out var actionHandler);

        // If no handler was found throw an exception.
        if (actionHandler == null)
        {
            throw new Exception($"No handler could be found for the Action of type: {action.GetType()}");
        }

        // Invoke the correct handler.
        return actionHandler.HandleAsync(action);
    }
}