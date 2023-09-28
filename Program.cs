using System.Reflection.Metadata.Ecma335;
using AirApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddLogging(l => l.AddConsole());
builder.Services.AddTransient<Api>();
builder.Configuration.AddJsonFile("appsettings.json");

var app = builder.Build();
app.MapControllers();

app.Run();
