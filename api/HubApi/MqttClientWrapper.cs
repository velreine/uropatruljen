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