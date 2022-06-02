using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using HubApi.Settings;
using MQTTnet;
using MQTTnet.Client;

namespace HubApi
{
    /// <summary>
    /// The Hub Api is responsible for controlling the Raspberry Pi that our physical components are attached to.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main entry point for our Hub Api.
        /// </summary>
        /// <param name="args">Command line arguments (if any) (not used).</param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            // Enables swagger to read triple-slash comments on endpoints and build documentation from that.
            builder.Services.AddSwaggerGen(swaggerGenOptions =>
            {
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                swaggerGenOptions.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            // Loads and registers our AppSettings as a service.
            var appSettings = LoadAndRegisterConfiguration(builder);

            if (appSettings.Mqtt == null)
            {
                throw new Exception("Cannot instantiate Hub Api without Mqtt settings.");
            }

            if (appSettings.Hardware == null)
            {
                throw new Exception("Cannot instantiate Api without proper Hardware settings.");
            }
            
            // Configure our Hub Api's Mqtt Client.
            ConfigureMqttClient(appSettings.Mqtt, appSettings.Hardware);

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

            app.Run();
        }

        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        private static AppSettings LoadAndRegisterConfiguration(WebApplicationBuilder builder)
        {
            // Read configuration.
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false)
                .Build();

            var appSettings = new AppSettings();
            config.GetSection("AppSettings").Bind(appSettings);
            
            // Register our AppSettings as a scoped service.
            // scoped: container will create an instance of the specified service type once per request and will be shared in a single request.
            builder.Services.AddScoped(provider => appSettings);
            
            Console.WriteLine("HubApi launches with the following settings:");
            appSettings.DumpToConsole();
            
            return appSettings;
        }

        private static void ConfigureMqttClient(MqttAppSettings mqttAppSettings, HardwareSettings hardwareSettings)
        {
            // Use the MqttClientOptionsBuild to build some client options.
            var mqttClientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer(mqttAppSettings.Endpoint)
                /*.WithTls(options =>
                {
                    options.SslProtocol = SslProtocols.Tls12;
                    options.AllowUntrustedCertificates = true;
                })*/
                .Build();

            // Instantiate our custom MqttClient wrapper which creates some default event handlers etc.
            var mqttClientWrapper = new MqttClientWrapper();


            // Connect the client to the server, then subscribe to the valid topics.
            mqttClientWrapper.Client
                .ConnectAsync(mqttClientOptions)
                .ContinueWith(previousConnectTask =>
                {
                    var factory = new MqttFactory();

                    // Build the options for the topics we are interested in subscribing to.
                    // The Hub Api is primarily interested in being "controlled" from the cloud.
                    var mqttSubscribeOptions = factory.CreateSubscribeOptionsBuilder()
                        .WithTopicFilter(filter =>
                        {
                            filter.WithTopic($"/device_actions/{hardwareSettings.SerialNumber}");
                        })
                        .Build();

                    // Subscribe to topics.
                    mqttClientWrapper.Client
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
    }
}