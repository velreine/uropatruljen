using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using CommonData.Logic.Factory;
using CommonData.Model.Action;
using HubApi.Handler;
using HubApi.Logic;
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

            builder.Services.AddTransient<IActionHandler<SetColorAction>, SetColorActionHandler>();
            builder.Services.AddTransient<IActionHandler<TurnOnOffAction>, TurnOnOffActionHandler>();
            builder.Services.AddTransient<IDefaultActionHandlers, DefaultActionHandlers>();
            builder.Services.AddSingleton<MqttClientWrapper, MqttClientWrapper>();

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
            
            var app = builder.Build();

            // Rider is wrong, this is used, the DI container will new() it and run the construct where the main logic to configure 
            // the client resides.
            var mqttClient = app.Services.GetService(typeof(MqttClientWrapper));

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
            builder.Services.AddTransient(provider => appSettings);
            
            Console.WriteLine("HubApi launches with the following settings:");
            appSettings.DumpToConsole();
            
            return appSettings;
        }
    }
}