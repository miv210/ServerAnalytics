using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.IdentityModel.Tokens;
using ServerAnalytics.Services;
using ServerAnalytics.Services.Interface;
using System.Reflection;
using ServerAnalytics.Services.AuthService;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Security.Cryptography.Xml;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // ���������, ����� �� �������������� �������� ��� ��������� ������
            ValidateIssuer = true,
            // ������, �������������� ��������
            ValidIssuer = AuthOptions.ISSUER,
            // ����� �� �������������� ����������� ������
            ValidateAudience = true,
            // ��������� ����������� ������
            ValidAudience = AuthOptions.AUDIENCE,
            // ����� �� �������������� ����� �������������
            ValidateLifetime = true,
            // ��������� ����� ������������
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            // ��������� ����� ������������
            ValidateIssuerSigningKey = true,
        };
    });

// Add services to the container.
builder.Services.AddControllers();


builder.Services.AddScoped<IMemoryMetricsService, MemoryMetricsService>();
builder.Services.AddScoped<IRuntimeInformation, RuntimeInformationService>();
builder.Services.AddScoped<IRunningProcessesService, RunningProcessService>();
builder.Services.AddScoped<IProcessorMetricsService, ProcessorMetricsService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddCors();


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseCors(builder => builder.AllowAnyOrigin());

app.UseAuthorization();

app.MapControllers();

app.Run();
