using System.Diagnostics.CodeAnalysis;
using FitTracker.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;
using Serilog;

namespace FitTracker.Api.Extensions
{
    /// <summary>
    /// WebApplication configuration extensions.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal static class WebApplicationExtensions
    {
        /// <summary>
        /// Configures the application pipeline and middleware.
        /// </summary>
        public static async Task<WebApplication> ConfigureApplicationAsync(
            this WebApplication app)
        {
            await app.MigrateDatabaseAsync();

            _ = app.UseSerilogRequestLogging(options =>
            {
                options.MessageTemplate =
                    "HTTP {RequestMethod} {RequestPath} responded {StatusCode} " +
                    "in {Elapsed:0.0000}ms";
                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    diagnosticContext.Set(
                        "RequestHost",
                        httpContext.Request.Host.Value);
                    diagnosticContext.Set(
                        "ClientIP",
                        httpContext.Connection.RemoteIpAddress);
                    diagnosticContext.Set(
                        "UserAgent",
                        httpContext.Request.Headers["User-Agent"].FirstOrDefault());
                };
            });

            if (app.Environment.IsDevelopment())
            {
                _ = app.MapOpenApi();
                _ = app.MapScalarApiReference(options =>
                {
                    options.Title = "FitTracker API";
                    options.AddPreferredSecuritySchemes("CookieAuth");
                });
            }

            if (app.Environment.IsDevelopment())
            {
                _ = app.UseHttpsRedirection();
            }

            _ = app.UseExceptionHandler();

            _ = app.UseCors("AllowAll");

            var locOptions = app.Services
                .GetRequiredService<IOptions<RequestLocalizationOptions>>();

            _ = app.UseRequestLocalization(locOptions.Value);

            _ = app.UseAuthentication();
            _ = app.UseAuthorization();

            _ = app.MapControllers();

            return app;
        }

        /// <summary>
        /// Applies pending database migrations.
        /// </summary>
        private static async Task MigrateDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
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
                throw;
            }
        }
    }
}
