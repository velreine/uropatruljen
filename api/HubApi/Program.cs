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

            // Enables swagger to read triple-slash comments on endpoints and build documentation from that.
            builder.Services.AddSwaggerGen(swaggerGenOptions =>
            {
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                swaggerGenOptions.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
            
            // Loads and registers our AppSettings as a service.
            LoadAndRegisterConfiguration(builder.Services);
            
            // Load our ActionHandlerContainer.
            RegisterActionHandlerContainer(builder.Services);

            // Register our MQTT Client wrapper.
            builder.Services.AddSingleton<MqttClientWrapper, MqttClientWrapper>();
            
            var app = builder.Build();

            // Ensure our singleton service MqttClientWrapper gets instantiated by the DI container.
            app.Services.GetService(typeof(MqttClientWrapper));
            
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

        private static void RegisterActionHandlerContainer(IServiceCollection services)
        {
            // Our ActionHandlers are services themselves.
            services.AddTransient<IActionHandler<SetColorAction>, SetColorActionHandler>();
            services.AddTransient<IActionHandler<TurnOnOffAction>, TurnOnOffActionHandler>();
            
            
            // Add our ActionHandlerContainer, which holds all our Action Handler Services.
            services.AddSingleton(sp =>
            {
                var ahl = new ActionHandlerContainer();
                ahl
                    .RegisterHandler(sp.GetService<IActionHandler<SetColorAction>>())
                    .RegisterHandler(sp.GetService<IActionHandler<TurnOnOffAction>>())
                    ;

                return ahl;
            });
        }
        
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        private static void LoadAndRegisterConfiguration(IServiceCollection services)
        {
            // Read configuration.
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false)
                .Build();

            var appSettings = new AppSettings();
            config.GetSection("AppSettings").Bind(appSettings);
            
            // Register our AppSettings as a scoped service.
            // scoped: container will create an instance of the specified service type once per request and will be shared in a single request.
            services.AddTransient(provider => appSettings);

            if (appSettings.Mqtt == null)
            {
                throw new Exception("Cannot instantiate Hub Api without Mqtt settings.");
            }

            if (appSettings.Hardware == null)
            {
                throw new Exception("Cannot instantiate Api without proper Hardware settings.");
            }
            
            Console.WriteLine("HubApi launches with the following settings:");
            appSettings.DumpToConsole();
        }
        
    }
}