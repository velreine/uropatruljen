using HubApi.Logic;
using HubApi.Model.Entity;
using HubApi.Model.State;
using HubApi.Model.Static;

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


// Manager gets and constructs the state and is responsible for "asking" the Arduino what the current state is.
var rgbState = new RgbComponentState(new Component());
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
        {new Pin(){Component = null, Direction = PinDirection.Input, Descriptor = "r_input", HwPinNumber = 10}},
        {new Pin(){Component = null, Direction = PinDirection.Input, Descriptor = "g_input", HwPinNumber = 11}},
        {new Pin(){Component = null, Direction = PinDirection.Input, Descriptor = "b_input", HwPinNumber = 12}},
    }
});

boardLayout.Add(DIODE_1, new Component()
{
    Id = DIODE_1,
    Type = ComponentType.Diode,
    Pins = new List<Pin>()
    {
        {new Pin(){Component = null, Direction = PinDirection.Input, Descriptor = "input", HwPinNumber = 8}},
    }
});

boardLayout.Add(DIODE_2, new Component()
{
    Id = DIODE_2,
    Type = ComponentType.RgbDiode,
    Pins = new List<Pin>()
    {
        {new Pin(){Component = null, Direction = PinDirection.Input, Descriptor = "input", HwPinNumber = 7}},
    }
});


var b = StringConverter.ToSnakeCase("APIHandler");
var x = StringConverter.ToSnakeCase("HardwareModel");
var y = StringConverter.ToSnakeCase("Hardware");
var z = StringConverter.ToSnakeCase("SMTPServer");
var a = StringConverter.ToSnakeCase("SmtpServer123");

app.Run();