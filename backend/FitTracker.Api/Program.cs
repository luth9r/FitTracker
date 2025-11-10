using FitTracker.Infrastructure;
using FitTracker.Infrastructure.Persistence.Data;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using FitTracker.Api.Extensions;

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(config)
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Configure services
    builder.ConfigureApplicationBuilder();

    var app = builder.Build();

    // Configure pipeline
    await app.ConfigureApplicationAsync();

    // Run application
    app.Run();

    Log.Information("FitTracker API stopped gracefully");
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
