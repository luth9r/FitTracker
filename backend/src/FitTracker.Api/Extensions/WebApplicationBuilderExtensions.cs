using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Security.Claims;
using System.Text;
using FitTracker.Api.Middlewares;
using FitTracker.Application;
using FitTracker.Application.Validators;
using FitTracker.Infrastructure;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using Serilog;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace FitTracker.Api.Extensions;

/// <summary>
///     WebApplicationBuilder configuration extensions.
/// </summary>
[ExcludeFromCodeCoverage]
internal static class WebApplicationBuilderExtensions
{
    /// <summary>
    ///     Configures all required services for the application.
    /// </summary>
    public static WebApplicationBuilder ConfigureApplicationBuilder(this WebApplicationBuilder builder)
    {
        _ = builder.Host.UseSerilog();

        _ = builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

        _ = builder.Services.AddProblemDetails();

        var supportedCultures = new[] { "en-US", "uk-UA" };

        _ = builder.Services.AddLocalization(options => { options.ResourcesPath = "Localization"; });

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
                    ValidIssuer = builder.Configuration["Jwt:Issuer"]
                                  ?? throw new ArgumentNullException("Jwt:Issuer"),
                    ValidAudience = builder.Configuration["Jwt:Audience"]
                                    ?? throw new ArgumentNullException("Jwt:Audience"),
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            builder.Configuration["Jwt:Key"]
                            ?? throw new ArgumentNullException("Jwt:Key"))),
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

        _ = builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy(
                "AuthenticatedWithVerifiedEmail",
                policy =>
                {
                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser()
                        .RequireClaim("is_email_verified", "true")
                        .RequireClaim(ClaimTypes.NameIdentifier);
                });

            options.FallbackPolicy = null;
        });

        _ = builder.Services.AddControllers();

        _ = builder.Services.AddValidatorsFromAssembly(typeof(LoginRequestValidator).Assembly);

        _ = builder.Services.AddFluentValidationAutoValidation();

        _ = builder.Services.AddOpenApi(options =>
        {
            options.AddScalarTransformers();

            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Components ??= new OpenApiComponents();
                document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();

                document.Components.SecuritySchemes.Add(
                    "CookieAuth",
                    new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.ApiKey,
                        In = ParameterLocation.Cookie,
                        Name = "auth-token",
                        Description = "JWT stored in http-only 'auth-token' cookie. Log in first to set the cookie.",
                    });

                document.SetReferenceHostDocument();

                return Task.CompletedTask;
            });

            options.AddOperationTransformer((operation, context, cancellationToken) =>
            {
                var authorizeAttributes = context.Description.ActionDescriptor.EndpointMetadata
                    .OfType<AuthorizeAttribute>()
                    .ToList();

                var allowAnonymous = context.Description.ActionDescriptor.EndpointMetadata
                    .OfType<AllowAnonymousAttribute>()
                    .Any();

                // If [Authorize] and without [AllowAnonymous]
                if (authorizeAttributes.Any() && !allowAnonymous)
                {
                    operation.Security = new List<OpenApiSecurityRequirement>
                    {
                        new()
                        {
                            {
                                new OpenApiSecuritySchemeReference("CookieAuth"),
                                new List<string>()
                            },
                        },
                    };
                }

                return Task.CompletedTask;
            });
        });

        _ = builder.Services.AddInfrastructure(builder.Configuration);

        _ = builder.Services.AddApplication(builder.Configuration);

        _ = builder.Services.AddCors(options =>
        {
            options.AddPolicy(
                "AllowAll",
                policy =>
                {
                    var allowedOrigins = builder.Configuration
                        .GetSection("Cors:AllowedOrigins")
                        .Get<string[]>() ?? new[] { "http://localhost:4200" };

                    policy.SetIsOriginAllowed(origin =>
                        {
                            // If it`s a mobile app, allow any origin
                            if (string.IsNullOrWhiteSpace(origin))
                            {
                                return true;
                            }

                            // If it`s a web app, allow only allowed origins
                            return allowedOrigins.Any(o => new Uri(o).Host == new Uri(origin).Host);
                        })
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
        });

        return builder;
    }
}
