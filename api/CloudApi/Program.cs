using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using CloudApi.Data;
using CloudApi.Logic;
using CloudApi.Repository;
using CloudApi.Settings;
using CommonData.Model.Entity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MQTTnet.AspNetCore;
using MQTTnet.Server;
// ReSharper disable ClassNeverInstantiated.Global

namespace CloudApi
{
    /// <summary>
    /// The Cloud Api is responsible for managing central user data, in addition the Cloud Api is also responsible for
    /// being a MQTT broker service that facilitates communication between MQTT clients.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main entry point for our Cloud Api.
        /// </summary>
        /// <param name="args"></param>
        /// <exception cref="Exception"></exception>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers().AddJsonOptions(x =>
            {
                // This means the serializer will automatically detect cycles/circular-references and ignore them.
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });
            
            // Resolve settings from different places and bind them to classes.
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false).Build();
            var jwtSettings = new JwtSettings();
            var dbSettings = new DatabaseSettings();
            config.GetSection("Database").Bind(dbSettings);
            config.GetSection("jwt").Bind(jwtSettings);
            
            // Do not launch the application if the database connection string is not set properly.
            if (dbSettings.ConnectionString == null || string.IsNullOrWhiteSpace(dbSettings.ConnectionString))
            {
                throw new Exception(
                    "The HubApi could not start, as the Database connection string is not configured properly." +
                    "It should be configured in appsettings.json, or through environment variables."
                );
            }

            // Do not launch the application if the Json Web Token settings are not configured properly.
            if (jwtSettings.Key == null || jwtSettings.Issuer == null)
            {
                throw new Exception(
                    "The HubApi could not start, as the Json Web Token settings are not configured properly." +
                    "It should be configured in appsettings.json, or through environment variables."
                );
            }

            builder.Services.AddSqlServer<UroContext>(dbSettings.ConnectionString, optionsBuilder =>
            {
            });

            // Add JSON Web Token Authentication
            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                    };
                });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(genOptions =>
            {
                genOptions.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                genOptions.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
                
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                genOptions.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            // Add our mock password hasher.
            builder.Services.AddTransient<IPasswordHasher<Person>, MockPasswordHasher>();
            builder.Services.AddTransient<HomeRepository, HomeRepository>(); // TODO: Fix, and replace with interface.
            builder.Services
                .AddTransient<DeviceRepository, DeviceRepository>(); // TODO: Fix, and replace with interface.
            builder.Services
                .AddTransient<PersonRepository, PersonRepository>(); // TODO: Fix, and replace with interface.
            builder.Services.AddTransient<HardwareLayoutRepository, HardwareLayoutRepository>();
            builder.Services.AddTransient<RoomRepository, RoomRepository>();


            // Configure our MQTT Server.
            var mqttServerOptions = new MqttServerOptionsBuilder()
                    .WithDefaultEndpoint()
                    .Build()
                ;

            builder.Services.AddHostedMqttServer(mqttServerOptions)
                .AddMqttConnectionHandler()
                .AddConnections()
                .AddMqttTcpServerAdapter();


            var app = builder.Build();

            // Configure MQTT Server callbacks.
            app.UseMqttServer(server =>
            {
                server.StartedAsync += (eventArgs) =>
                {
                    Console.WriteLine("The MQTT Server has started.");
                    return Task.CompletedTask;
                };

                server.StoppedAsync += (eventArgs) =>
                {
                    // Implement if necessary.
                    Console.WriteLine("The MQTT Server has stopped.");
                    return Task.CompletedTask;
                };

                server.ClientConnectedAsync += eventArgs =>
                {
                    Console.WriteLine(
                        $"A client has connected ClientId: {eventArgs.ClientId}, Endpoint:{eventArgs.Endpoint}, Username: {eventArgs.UserName}");
                    return Task.CompletedTask;
                };

                server.ClientDisconnectedAsync += (eventArgs) =>
                {
                    var disconnectReason = eventArgs.DisconnectType switch
                    {
                        MqttClientDisconnectType.Clean => "Clean",
                        MqttClientDisconnectType.Takeover => "TakeOver",
                        MqttClientDisconnectType.NotClean => "Not Clean",
                        _ => "Unknown"
                    };

                    Console.WriteLine(
                        $"A client has disconnected ClientId: {eventArgs.ClientId}, Endpoint:{eventArgs.Endpoint}, Reason: {disconnectReason}");
                    return Task.CompletedTask;
                };

                server.ClientSubscribedTopicAsync += (eventArgs) =>
                {
                    Console.WriteLine(
                        $"A client subscribed to topic ClientId:{eventArgs.ClientId}, Topic:{eventArgs.TopicFilter.Topic}");
                    return Task.CompletedTask;
                };

                server.ClientUnsubscribedTopicAsync += (eventArgs) =>
                {
                    Console.WriteLine(
                        $"A client unsubscribed to topic ClientId:{eventArgs.ClientId}, Topic:{eventArgs.TopicFilter}");
                    return Task.CompletedTask;
                };
            });

            // This is necessary for the AspNetCore Server to function properly behind a reverse proxy.
            // Apache/Nginx etc..
            app.UseForwardedHeaders(new ForwardedHeadersOptions()
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Enables the configured authentication (JWT for us).
            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}