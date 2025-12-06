using FitTracker.Application;
using FitTracker.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Globalization;
using System.Text;

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
            _ = builder.Host.UseSerilog();

            var supportedCultures = new[] { "en-US", "uk-UA" };

            _ = builder.Services.AddLocalization(options =>
            {
                options.ResourcesPath = "Localization";
            });

            _ = builder.Services.Configure<RequestLocalizationOptions>(options =>
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
                    new AcceptLanguageHeaderRequestCultureProvider(),
                };
            });

            _ = builder.Services.AddRouting(options => options.LowercaseUrls = true);

            _ = builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new ArgumentNullException())),
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["auth-token"];
                            return Task.CompletedTask;
                        },
                    };
                });

            _ = builder.Services.AddAuthorization();

            _ = builder.Services.AddControllers();

            _ = builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "FitTracker API",
                    Version = "v1",
                    Description = "API for FitTracker",
                    Contact = new OpenApiContact
                    {
                        Name = "FitTracker Team",
                        Email = "support@fittracker.com",
                    },
                });
            });

            _ = builder.Services.AddInfrastructure(builder.Configuration);

            _ = builder.Services.AddApplication(builder.Configuration);

            _ = builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    var allowedOrigins = builder.Configuration
                        .GetSection("Cors:AllowedOrigins")
                        .Get<string[]>() ?? new[] { "http://localhost:4200" };

                    _ = policy.WithOrigins(allowedOrigins)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            return builder;
        }
    }
}
