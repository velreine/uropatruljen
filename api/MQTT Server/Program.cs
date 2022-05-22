using MQTTnet;
using MQTTnet.Client.Receiving;
using MQTTnet.Server;

namespace MQTT_Server
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            var handler = new MqttHandler();

            await handler.DoStuff();
        }

    }


    public class MqttHandler
    {

        private DefaultMqttEventHandlers _handlers = new DefaultMqttEventHandlers();

        public async Task DoStuff()
        {
            Console.WriteLine("MQTT Server!");

            var factory = new MqttFactory();

            // The port for the default endpoint is 1883.
            // The default endpoint is NOT encrypted!
            // Use the builder classes where possible.
            var options = new MqttServerOptionsBuilder()
                .WithDefaultEndpoint()
                .Build();

            using (var server = factory.CreateMqttServer(/*mqttServerOptions*/))
            {

                // Subscribe our default handler methods to the different events.
                server.ApplicationMessageReceivedHandler = _handlers;
                server.ClientDisconnectedHandler = _handlers;
                server.ClientSubscribedTopicHandler = _handlers;
                server.ClientUnsubscribedTopicHandler = _handlers;


                await server.StartAsync(options);

                Console.WriteLine("Any key to send message, Q to stop sending messages.");
                ConsoleKeyInfo? lastPressed = null;

                while (lastPressed == null || lastPressed?.Key != ConsoleKey.Q)
                {
                    lastPressed = Console.ReadKey();

                    var msg = new MqttApplicationMessage();
                    msg.Topic = "mqttnet/samples/topic/2";
                    msg.Payload = new[] { (byte)0xFF/*, (byte)0xAD, (byte)0xBE, (byte)0xEF*/ };

                    Console.WriteLine($"Sending message to subscribers of {msg.Topic}");
                    await server.PublishAsync(msg, CancellationToken.None);


                }

                Console.WriteLine("Press Enter to exit.");
                Console.ReadLine();

                // Stop and dispose the MQTT server if it is no longer needed!
                await server.StopAsync();
            }
        }

        private class DefaultMqttEventHandlers : IMqttServerClientConnectedHandler , IMqttServerClientDisconnectedHandler, IMqttServerClientSubscribedTopicHandler, IMqttServerClientUnsubscribedTopicHandler , IMqttApplicationMessageReceivedHandler
        {
            public Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs eventArgs)
            {
                Console.WriteLine($"An application message was received from client.");

                eventArgs.ApplicationMessage.DumpToConsole();

                return Task.CompletedTask;
            }

            public Task HandleClientConnectedAsync(MqttServerClientConnectedEventArgs eventArgs)
            {
                Console.WriteLine($"A client connected, Username: {eventArgs.UserName}, ClientId: {eventArgs.ClientId}, Endpoint: {eventArgs.Endpoint}, ProtocolVersion: {eventArgs.ProtocolVersion}");

                return Task.CompletedTask;
            }

            public Task HandleClientDisconnectedAsync(MqttServerClientDisconnectedEventArgs eventArgs)
            {
                Console.WriteLine($"A client disconnected, ClientId: {eventArgs.ClientId}, DisconnectType: {eventArgs.DisconnectType}, Endpoint: {eventArgs.Endpoint}");

                return Task.CompletedTask;
            }

            public Task HandleClientSubscribedTopicAsync(MqttServerClientSubscribedTopicEventArgs eventArgs)
            {
                Console.WriteLine($"A client subscribed to a topic, ClientId: {eventArgs.ClientId}, Topic: {eventArgs.TopicFilter.Topic}");

                return Task.CompletedTask;
            }

            public Task HandleClientUnsubscribedTopicAsync(MqttServerClientUnsubscribedTopicEventArgs eventArgs)
            {
                Console.WriteLine($"A client un-subscribed from a topic, ClientId: {eventArgs.ClientId}, Topic: {eventArgs.TopicFilter}");

                return Task.CompletedTask;
            }
        }

    }

}