using CommonData.Model.Entity;
using CommonData.Model.Static;
using HubApi;
using HubApi.Settings;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Read configuration.
var config = new ConfigurationBuilder().AddJsonFile("appsettings.json", false).Build();
var mqttAppSettings = new MqttAppSettings();
var hardwareSettings = new HardwareSettings();
config.GetSection("MqttAppSettings").Bind(mqttAppSettings);
config.GetSection("HardwareSettings").Bind(hardwareSettings);

Console.WriteLine("---BEGINNING OF CURRENT SETTINGS---");
mqttAppSettings.DumpToConsole();
hardwareSettings.DumpToConsole();
Console.WriteLine("---END OF CURRENT SETTINGS---");

// Configure our MQTT client.
var mqttClientOptions = new MqttClientOptionsBuilder()
    .WithTcpServer(mqttAppSettings.Endpoint)
    /*.WithTls(options =>
    {
        options.SslProtocol = SslProtocols.Tls12;
        options.AllowUntrustedCertificates = true;
    })*/
    .Build();

var factory = new MqttFactory();
var client = factory.CreateMqttClient();
client.ConnectedAsync += (eventArgs) =>
{
    Console.WriteLine("The client has successfully connected to the server.");
    eventArgs.DumpToConsole();
    return Task.CompletedTask;
};

client.DisconnectedAsync += (eventArgs) =>
{
    var reason = Enum.GetName(eventArgs.Reason);

    Console.WriteLine($"The client has disconnected, Reason: {reason}");
    Console.WriteLine(eventArgs.Exception.Message);

    // Keep trying to connect to the server in intervals of 5 seconds.
    /*while (client.IsConnected != true)
    {
        client.ConnectAsync(mqttClientOptions);
        Thread.Sleep(5000);
    }*/

    return Task.CompletedTask;
};

client.ConnectingAsync += (eventArgs) =>
{
    Console.WriteLine("The client is connecting...");
    eventArgs.DumpToConsole();
    return Task.CompletedTask;
};

client.ApplicationMessageReceivedAsync += (eventArgs) =>
{
    Console.WriteLine("The client received an application message.");
    eventArgs.DumpToConsole();
    return Task.CompletedTask;
};

// Connect the client, then subscribe to the valid topics.
// Then output to the console.
client
    .ConnectAsync(mqttClientOptions)
    .ContinueWith(previousConnectTask =>
    {
        // Build the options for the topics we are interested in subscribing to.
        // The Hub Api is primarily interested in being "controlled" from the cloud.
        var mqttSubscribeOptions = factory.CreateSubscribeOptionsBuilder()
            .WithTopicFilter(filter =>
            {
                filter.WithTopic($"/device_actions/{hardwareSettings.SerialNumber}");
            })
            .Build();

        client
            .SubscribeAsync(mqttSubscribeOptions, CancellationToken.None)
            .ContinueWith(previousSubscribeTask =>
            {
                Console.WriteLine("The client successfully subscribed to its topics!");
                previousSubscribeTask.Result.DumpToConsole();
                return Task.CompletedTask;
            });

        return Task.CompletedTask;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Manager gets and constructs the state and is responsible for "asking" the Arduino what the current state is.
var rgbState = new RgbComponentState();
rgbState.Component = new Component();
rgbState.IsOn = true;
rgbState.RValue = 200;
rgbState.GValue = 100;
rgbState.BValue = 50;

// public ComponentState GetStateOfComponent(Component component) {
//  var stateRequest = new object { componentIdentifier = component.Id }
//  var response = SomeManager.SendCommand(stateRequest);
//  var state = CreateStateFromResponse(response);
//  return state;
// }


const int RGB_DIODE_1 = 1;
const int DIODE_1 = 2;
const int DIODE_2 = 3;
var boardLayout = new Dictionary<int, Component>();

// Add RGB_DIODE_1
boardLayout.Add(RGB_DIODE_1, new Component()
{
    Id = RGB_DIODE_1,
    Type = ComponentType.RgbDiode,
    Pins = new List<Pin>()
    {
        { new Pin() { Component = null, Direction = PinDirection.Input, Descriptor = "r_input", HwPinNumber = 10 } },
        { new Pin() { Component = null, Direction = PinDirection.Input, Descriptor = "g_input", HwPinNumber = 11 } },
        { new Pin() { Component = null, Direction = PinDirection.Input, Descriptor = "b_input", HwPinNumber = 12 } },
    }
});

boardLayout.Add(DIODE_1, new Component()
{
    Id = DIODE_1,
    Type = ComponentType.Diode,
    Pins = new List<Pin>()
    {
        { new Pin() { Component = null, Direction = PinDirection.Input, Descriptor = "input", HwPinNumber = 8 } },
    }
});

boardLayout.Add(DIODE_2, new Component()
{
    Id = DIODE_2,
    Type = ComponentType.RgbDiode,
    Pins = new List<Pin>()
    {
        { new Pin() { Component = null, Direction = PinDirection.Input, Descriptor = "input", HwPinNumber = 7 } },
    }
});

app.Run();