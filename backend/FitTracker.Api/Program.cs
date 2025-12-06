using FitTracker.Api.Extensions;
using Serilog;

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
    _ = builder.ConfigureApplicationBuilder();

    var app = builder.Build();

    // Configure pipeline
    _ = await app.ConfigureApplicationAsync();

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
