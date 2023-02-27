
using ServerAnalytics.Services;
using ServerAnalytics.Services.Interface;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IMemoryMetricsService, MemoryMetricsService>();
builder.Services.AddScoped<IRuntimeInformation, RuntimeInformationService>();
builder.Services.AddScoped<IRunningProcessesService, RunningProcessService>();


var app = builder.Build();

// Configure the HTTP request pipeline.


app.UseAuthorization();

app.MapControllers();

app.Run();
