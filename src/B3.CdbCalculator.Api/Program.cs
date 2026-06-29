using B3.CdbCalculator.Core.Interfaces;
using B3.CdbCalculator.Core.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Serviços de domínio
builder.Services.AddScoped<ITaxRateService, TaxRateService>();
builder.Services.AddScoped<ICdbCalculatorService, CdbCalculatorService>();

// API
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();

// CORS
var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>() ?? ["http://localhost:4200"];

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseExceptionHandler();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors();
app.MapControllers();

await app.RunAsync();
