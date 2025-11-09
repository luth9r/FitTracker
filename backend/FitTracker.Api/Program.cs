using FitTracker.Infrastructure;
using FitTracker.Infrastructure.Persistence.Data;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
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


    var supportedCultures = new[] { "en-US", "uk-UA" };

    builder.Services.AddLocalization(options =>
    {
        options.ResourcesPath = "Localization";
    });

    builder.Services.Configure<RequestLocalizationOptions>(options =>
    {
        options.DefaultRequestCulture = new RequestCulture("en-US");
        options.SupportedCultures = supportedCultures
            .Select(c => new System.Globalization.CultureInfo(c))
            .ToList();
        options.SupportedUICultures = supportedCultures
            .Select(c => new System.Globalization.CultureInfo(c))
            .ToList();

        options.RequestCultureProviders = new List<IRequestCultureProvider>
        {
            new QueryStringRequestCultureProvider(),           // ?culture=uk-UA
            new CookieRequestCultureProvider(),                 // Cookie
            new AcceptLanguageHeaderRequestCultureProvider()    // Accept-Language header
        };
    });

    // ============================================
    // Add Serilog to the host
    // ============================================
    builder.Host.UseSerilog();

    // Services
    builder.Services.AddControllers();


    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "FitTracker API",
            Version = "v1",
            Description = "API for FitTracker",
            Contact = new OpenApiContact
            {
                Name = "FitTracker Team",
                Email = "support@fittracker.com"
            }
        });
    });

    // Infrastructure и Application services
    builder.Services.AddInfrastructure(builder.Configuration);

    // CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins")
                .Get<string[]>() ?? ["http://localhost:4200"];

            policy.WithOrigins(allowedOrigins)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
    });


    // AutoMapper



    var app = builder.Build();

    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<FitTrackerDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        try
        {
            logger.LogInformation("Starting database migrations...");
            await db.Database.MigrateAsync();
            logger.LogInformation("Database migrations applied successfully!");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Database initialization error");
        }
    }

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "FitTracker API v1");
            c.RoutePrefix = string.Empty; // Swagger for root URL
        });
    }

    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate =
            "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000}ms";
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
            diagnosticContext.Set("ClientIP", httpContext.Connection.RemoteIpAddress);
            diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].FirstOrDefault());
        };
    });

    if (app.Environment.IsDevelopment())
    {
        app.UseHttpsRedirection();
    }

    // Exception handling middleware
    app.UseExceptionHandler("/error");

    // CORS
    app.UseCors("AllowAll");

    // ============================================
    // Use Localization Middleware
    // ============================================
    var locOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
    app.UseRequestLocalization(locOptions.Value);

    // Authentication & Authorization
    app.UseAuthentication();
    app.UseAuthorization();

    // Controllers routing
    app.MapControllers();


    // ============================================
    // Run the application
    // ============================================
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