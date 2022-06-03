using CommonData.Logic.Factory;
using CommonData.Model.Action;
using HubApi.Logic;
using HubApi.Settings;
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
    private IDefaultActionHandlers _actionHandlers;
    private DefaultActionFactory _actionFactory = new DefaultActionFactory();

    /// <summary>
    /// This class is a wrapper class for the MQTTnet MqttClient.
    /// It registers some default event handlers to the client upon construction.
    /// </summary>
    public MqttClientWrapper(IDefaultActionHandlers actionHandlers, AppSettings appSettings)
    {
        _actionHandlers = actionHandlers;
        // Create our underlying client and register our event handlers.
        var factory = new MqttFactory();
        Client = factory.CreateMqttClient();
        Client.ConnectedAsync += ClientOnConnectedAsync;
        Client.DisconnectedAsync += ClientOnDisconnectedAsync;
        Client.ConnectingAsync += ClientOnConnectingAsync;
        Client.ApplicationMessageReceivedAsync += ClientOnApplicationMessageReceivedAsync;
        
        // Use the MqttClientOptionsBuild to build some client options.
        var mqttClientOptions = new MqttClientOptionsBuilder()
            .WithTcpServer(appSettings.Mqtt!.Endpoint)
            /*.WithTls(options =>
            {
                options.SslProtocol = SslProtocols.Tls12;
                options.AllowUntrustedCertificates = true;
            })*/
            .Build();
        
        // Connect the client to the server, then subscribe to the valid topics.
        Client
            .ConnectAsync(mqttClientOptions)
            .ContinueWith(_ =>
            {
                // Build the options for the topics we are interested in subscribing to.
                // The Hub Api is primarily interested in being "controlled" from the cloud.
                var mqttSubscribeOptions = factory.CreateSubscribeOptionsBuilder()
                    .WithTopicFilter(filter =>
                    {
                        filter.WithTopic($"/device_actions/{appSettings.Hardware!.SerialNumber}");
                    })
                    .Build();

                // Subscribe to topics.
                Client
                    .SubscribeAsync(mqttSubscribeOptions, CancellationToken.None)
                    .ContinueWith(previousSubscribeTask =>
                    {
                        Console.WriteLine("The client successfully subscribed to its topics!");
                        previousSubscribeTask.Result.DumpToConsole();
                        return Task.CompletedTask;
                    });

                return Task.CompletedTask;
            });
        
    }

    private Task ClientOnApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs arg)
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
        _actionHandlers.AssignToHandler(action);

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