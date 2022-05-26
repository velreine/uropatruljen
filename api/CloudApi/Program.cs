using System.Text.Json.Serialization;
using CloudApi.Data;

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
            });


            // TODO: Replace hardcoded server connection string with a fetch from ENVIRONMENT VARIABLE, or fallback to
            // lookup in config file?
            //using var conn = new SqlConnection("Server=localhost;Database=uro_db;User Id=sa;Password=12345");
            builder.Services.AddSqlServer<UroContext>("Server=localhost;Database=uro_db;User Id=sa;Password=12345",
                optionsBuilder =>
                {
                    // TODO: how to add snake_case naming strategy?
                    // https://docs.microsoft.com/en-us/ef/core/querying/single-split-queries
                });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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
    }
}