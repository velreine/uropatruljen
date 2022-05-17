using HubApi.Logic;
using HubApi.Model.Entity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
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


var data = new { id = 1, name = "Foobar Hjem", };


var home = ScalarObjectHydrator.HydrateType<Home>(data);

var x = StringConverter.ToSnakeCase("HardwareModel");
var y = StringConverter.ToSnakeCase("Hardware");
var z = StringConverter.ToSnakeCase("SMTPServer");
var a = StringConverter.ToSnakeCase("SmtpServer123");

app.Run();