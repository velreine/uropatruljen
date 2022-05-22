using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Formatter;
using MQTTnet.Extensions;
using System.Text.Json;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet;
using MQTTnet.Client.Receiving;

namespace MQTT_Client
{
    internal class Program
    {

        static async Task Main(string[] args)
        {
            Console.WriteLine("MQTT Client!");

            var handler = new MqttHandler();

            await handler.DoStuff();
        }

    }

    public class MqttHandler
    {
        private DefaultMqttEventHandlers _handlers = new DefaultMqttEventHandlers();

        private class DefaultMqttEventHandlers : IMqttClientConnectedHandler , IMqttClientDisconnectedHandler, IMqttApplicationMessageReceivedHandler
        {
            public Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs eventArgs)
            {
                Console.WriteLine("Got message:");

                eventArgs.ApplicationMessage.DumpToConsole();

                return Task.CompletedTask;
            }

            public Task HandleConnectedAsync(MqttClientConnectedEventArgs eventArgs)
            {

                Console.WriteLine("The MQTT client is connectedXXX.");

                eventArgs.ConnectResult.DumpToConsole();

                return Task.CompletedTask;
            }

            public Task HandleDisconnectedAsync(MqttClientDisconnectedEventArgs eventArgs)
            {
                Console.WriteLine("The MQTT client is disconnectedXXX.");

                eventArgs.ConnectResult.DumpToConsole();  

                return Task.CompletedTask;
            }



        }

        public async Task DoStuff()
        {
            // Initialize client.
            var factory = new MqttFactory();
            using var client = factory.CreateMqttClient();
            var clientOptions = new MqttClientOptionsBuilder().WithTcpServer("127.0.0.1").Build();

            // Subscribe our handler methods to the different events.
            client.ConnectedHandler = this._handlers;
            client.DisconnectedHandler = this._handlers;
            client.ApplicationMessageReceivedHandler = this._handlers;
            

            // Connect the client to the server.
            var response = await client.ConnectAsync(clientOptions, CancellationToken.None);

            // Subscribe to a topic?
            Console.WriteLine("Trying to subscribe....");
            var subscribeOptions = factory.CreateSubscribeOptionsBuilder()
               .WithTopicFilter(f => { f.WithTopic("mqttnet/samples/topic/2"); })
               .Build();
            var subscribeResult = await client.SubscribeAsync(subscribeOptions, CancellationToken.None);
            Console.WriteLine("MQTT client subscribed to topic.");
            subscribeResult.DumpToConsole();

            // Dump the result for debugging.
            response.DumpToConsole();


            Console.WriteLine("Press enter to disconnect!");
            Console.ReadLine();

            // Gracefully disconnect the client.
            var disconnectOptions = new MqttClientDisconnectOptions();
            await client.DisconnectAsync(disconnectOptions, CancellationToken.None);
        }

    }
}