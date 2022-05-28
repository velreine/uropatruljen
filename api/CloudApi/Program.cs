using System.Text;
using System.Text.Json.Serialization;
using CloudApi.Data;
using CloudApi.Logic;
using CloudApi.Repository;
using CommonData.Model.Entity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace CloudApi
{
    public class Program
    {


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


            // Read JSON Web Token settings from configuration.
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false).Build();
            JwtSettings jwtSettings = new JwtSettings();
            config.GetSection("jwt").Bind(jwtSettings);
            

            // TODO: Replace hardcoded server connection string with a fetch from ENVIRONMENT VARIABLE, or fallback to
            // lookup in config file?
            //using var conn = new SqlConnection("Server=localhost;Database=uro_db;User Id=sa;Password=12345");
            builder.Services.AddSqlServer<UroContext>("Server=localhost; Database=uro_db; User Id=sa; Password=12345; Trusted_Connection=True; TrustServerCertificate=True;",
                optionsBuilder =>
                {
                    // TODO: how to add snake_case naming strategy?
                    // https://docs.microsoft.com/en-us/ef/core/querying/single-split-queries
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
                
            });

            // Add our mock password hasher.
            builder.Services.AddTransient<IPasswordHasher<Person>, MockPasswordHasher>();
            builder.Services.AddTransient<HomeRepository, HomeRepository>(); // TODO: Fix, and replace with interface.
            builder.Services.AddTransient<DeviceRepository, DeviceRepository>(); // TODO: Fix, and replace with interface.
            builder.Services.AddTransient<PersonRepository, PersonRepository>(); // TODO: Fix, and replace with interface.
            
            var app = builder.Build();

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