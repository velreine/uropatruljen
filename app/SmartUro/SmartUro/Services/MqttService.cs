using MQTTnet;
using MQTTnet.Client;
using SmartUro.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommonData.Model.Action;

namespace SmartUro.Services
{
    internal class MqttService : IMqttService
    {
        private MqttClient _mqttClient;

        public MqttService()
        {
            // Configure MQTT client options.
            var mqttClientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer("mqtt.uroapp.dk")
                .Build();

            var mqttFactory = new MqttFactory();
            var client = mqttFactory.CreateMqttClient();

            _mqttClient = client;

            // Register event handlers before doing the actual connection..
            _mqttClient.ConnectedAsync += (eventArgs) =>
            {
                Debug.WriteLine("MQTT: The client has successfully connected to the server.");
                //eventArgs.DumpToConsole();
                return Task.CompletedTask;
            };

            _mqttClient.DisconnectedAsync += (eventArgs) =>
            {
                var reason = Enum.GetName(typeof(MqttClientDisconnectReason), eventArgs.Reason);

                Debug.WriteLine($"MQTT: The client has disconnected, Reason: {reason}");
                Debug.WriteLine(eventArgs.Exception.Message);

                // Keep trying to connect to the server in intervals of 5 seconds.
                /*while (client.IsConnected != true)
                {
                    client.ConnectAsync(mqttClientOptions);
                    Thread.Sleep(5000);
                }*/

                return Task.CompletedTask;
            };

            _mqttClient.ConnectingAsync += (eventArgs) =>
            {
                Debug.WriteLine("MQTT: The client is connecting...");
                //eventArgs.DumpToConsole();
                return Task.CompletedTask;
            };

            _mqttClient.ApplicationMessageReceivedAsync += (eventArgs) =>
            {
                // TODO: Implement handlers...
                Debug.WriteLine("MQTT: The client received an application message.");
                //eventArgs.DumpToConsole();
                return Task.CompletedTask;
            };

            //Connect
            _mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);
        }

        public async Task SendRequest()
        {
            Debug.WriteLine("SEND REQUEST");

            var action = new TurnOnOffAction();
            action.ComponentIdentifier = 1; // TODO: Grab from hardware layout of the....
            

            /*var message = new MqttApplicationMessage();
            message.Payload = new byte[] { 0xDE, 0xAD, 0xBE, 0xEF };
            message.Topic = "/device_actions/SN-ABC123";
            await _mqttClient.PublishAsync(message, CancellationToken.None);*/
        }
    }
}
