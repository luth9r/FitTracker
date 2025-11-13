using FitTracker.Application;
using FitTracker.Infrastructure;
using Microsoft.AspNetCore.Localization;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Globalization;

namespace FitTracker.Api.Extensions
{
    /// <summary>
    /// WebApplicationBuilder configuration extensions.
    /// </summary>
    public static class WebApplicationBuilderExtensions
    {
        /// <summary>
        /// Configures all required services for the application.
        /// </summary>
        public static WebApplicationBuilder ConfigureApplicationBuilder(
            this WebApplicationBuilder builder)
        {
            #region Logging

            builder.Host.UseSerilog();

            #endregion

            #region Localization

            var supportedCultures = new[] { "en-US", "uk-UA" };

            builder.Services.AddLocalization(options =>
            {
                options.ResourcesPath = "Localization";
            });

            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("en-US");
                options.SupportedCultures = supportedCultures
                    .Select(c => new CultureInfo(c))
                    .ToList();
                options.SupportedUICultures = supportedCultures
                    .Select(c => new CultureInfo(c))
                    .ToList();

                options.RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new QueryStringRequestCultureProvider(),
                    new CookieRequestCultureProvider(),
                    new AcceptLanguageHeaderRequestCultureProvider()
                };
            });

            #endregion

            #region Routing

            builder.Services.AddRouting(options => options.LowercaseUrls = true);

            #endregion

            #region Controllers

            builder.Services.AddControllers();

            #endregion

            #region Swagger

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

            #endregion

            #region Infrastructure

            builder.Services.AddInfrastructure(builder.Configuration);

            #endregion

            #region Application

            builder.Services.AddApplication(builder.Configuration);

            #endregion

            #region CORS

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    var allowedOrigins = builder.Configuration
                        .GetSection("Cors:AllowedOrigins")
                        .Get<string[]>() ?? new[] { "http://localhost:4200" };

                    policy.WithOrigins(allowedOrigins)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            #endregion

            return builder;
        }
    }
}
