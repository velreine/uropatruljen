using CommonData.Logic.Factory;
using CommonData.Model.Action;
using HubApi.Logic;
using MQTTnet;
using MQTTnet.Client;

namespace HubApi;

/**
 * Wrapper class for an Mqtt Client that provides our custom event handlers.
 */
public class MqttClientWrapper
{
    /// <summary>
    /// The client that was wrapped.
    /// </summary>
    public MqttClient Client { get; set; }

    // TODO: Maybe these DefaultActionHandlers should be registered as a service,
    // And injected through the constructor, i suppose this MqttClientWrapper could also be a service.
    private static DefaultActionHandlers _actionHandlers = new DefaultActionHandlers();
    private static DefaultActionFactory _actionFactory = new DefaultActionFactory();

    /// <summary>
    /// This class is a wrapper class for the MQTTnet MqttClient.
    /// It registers some default event handlers to the client upon construction.
    /// </summary>
    public MqttClientWrapper()
    {
        // Create our underlying client and register our event handlers.
        var factory = new MqttFactory();
        Client = factory.CreateMqttClient();
        Client.ConnectedAsync += ClientOnConnectedAsync;
        Client.DisconnectedAsync += ClientOnDisconnectedAsync;
        Client.ConnectingAsync += ClientOnConnectingAsync;
        Client.ApplicationMessageReceivedAsync += ClientOnApplicationMessageReceivedAsync;
    }

    private static Task ClientOnApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs arg)
    {

        Console.WriteLine("The client received an application message.");
        arg.DumpToConsole();
        
        // Create the ActionPayload from the MQTT Application Message's Payload.
        var actionPayload = ActionPayload.FromPayload(arg.ApplicationMessage.Payload);
        
        // Grab the correct action type from the map according to the identifier in the payload.
        var actionType = ActionMap.ActionIdentifierToActionType[actionPayload.ActionIdentifier];
        
        // Now instruct the factory to instantiate that type of action.
        var action = _actionFactory.CreateAction(actionPayload.ActionData, actionType);
        
        // Finally, pass on the action to the correct handler.
        _actionHandlers.HandleAsync(action);

        return Task.CompletedTask;
    }

    private static Task ClientOnConnectingAsync(MqttClientConnectingEventArgs arg)
    {
        Console.WriteLine("The client is connecting...");
        arg.DumpToConsole();
        return Task.CompletedTask;
    }

    private static Task ClientOnDisconnectedAsync(MqttClientDisconnectedEventArgs arg)
    {
        var reason = Enum.GetName(arg.Reason);

        Console.WriteLine($"The client has disconnected, Reason: {reason}");
        Console.WriteLine(arg.Exception.Message);

        // Keep trying to connect to the server in intervals of 5 seconds.
        /*while (client.IsConnected != true)
        {
            client.ConnectAsync(mqttClientOptions);
            Thread.Sleep(5000);
        }*/

        return Task.CompletedTask;
    }

    private static Task ClientOnConnectedAsync(MqttClientConnectedEventArgs arg)
    {
        Console.WriteLine("The client has successfully connected to the server.");
        arg.DumpToConsole();
        return Task.CompletedTask;
    }


    
}