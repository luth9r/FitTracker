using FitTracker.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;

namespace FitTracker.Api.Extensions
{
    /// <summary>
    /// WebApplication configuration extensions.
    /// </summary>
    public static class WebApplicationExtensions
    {
        /// <summary>
        /// Configures the application pipeline and middleware.
        /// </summary>
        public static async Task<WebApplication> ConfigureApplicationAsync(
            this WebApplication app)
        {
            #region Database Migration

            await app.MigrateDatabaseAsync();

            #endregion

            #region Logging

            app.UseSerilogRequestLogging(options =>
            {
                options.MessageTemplate =
                    "HTTP {RequestMethod} {RequestPath} responded {StatusCode} " +
                    "in {Elapsed:0.0000}ms";
                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    diagnosticContext.Set("RequestHost",
                        httpContext.Request.Host.Value);
                    diagnosticContext.Set("ClientIP",
                        httpContext.Connection.RemoteIpAddress);
                    diagnosticContext.Set("UserAgent",
                        httpContext.Request.Headers["User-Agent"].FirstOrDefault());
                };
            });

            #endregion

            #region Swagger

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FitTracker API v1");
                    c.RoutePrefix = string.Empty;
                });
            }

            #endregion

            #region Security

            if (app.Environment.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }

            #endregion

            #region Exception Handling

            app.UseExceptionHandler("/error");

            #endregion

            #region CORS

            app.UseCors("AllowAll");

            #endregion

            #region Localization

            var locOptions = app.Services
                .GetRequiredService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);

            #endregion

            #region Authentication & Authorization

            app.UseAuthentication();
            app.UseAuthorization();

            #endregion

            #region Endpoints

            app.MapControllers();

            #endregion

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
