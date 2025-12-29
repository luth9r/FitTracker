using System.Diagnostics.CodeAnalysis;
using FitTracker.Application.Interfaces;
using FitTracker.Application.UseCases.User.Commands;
using FitTracker.Domain.Abstract.Interfaces;
using FitTracker.Infrastructure.Localization;
using FitTracker.Infrastructure.Persistence.Data;
using FitTracker.Infrastructure.Persistence.Repositories;
using FitTracker.Infrastructure.Services;
using FitTracker.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FitTracker.Infrastructure;

[ExcludeFromCodeCoverage]
public static class InfrastructureInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        AddDatabase(services, configuration);

        AddLocalization(services);

        AddRepositories(services);

        AddAuthAndTokens(services, configuration);

        AddAutoMappers(services);

        return services;
    }

    private static void AddDatabase(
        IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException(
                "Database connection string 'DefaultConnection' is not configured.");
        }

        _ = services.AddDbContext<FitTrackerDbContext>(options =>
        {
            _ = options.UseNpgsql(
                connectionString,
                npgsqlOptions =>
                {
                    _ = npgsqlOptions.MigrationsAssembly("FitTracker.Infrastructure");
                    _ = npgsqlOptions.CommandTimeout(30);
                    _ = npgsqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorCodesToAdd: null);
                });

            var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

            if (isDevelopment)
            {
                _ = options.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            }
        });
    }

    private static void AddLocalization(IServiceCollection services)
    {
        _ = services.AddHttpContextAccessor();

        _ = services.AddSingleton<ILocalizationProvider, JsonLocalizationProvider>();

        _ = services.AddScoped<ILocalizationService, LocalizationService>();
    }

    private static void AddRepositories(IServiceCollection services)
    {
        _ = services.AddScoped<IUnitOfWork, UnitOfWork>();
        _ = services.AddScoped<IUserWriteRepository, UserWriteRepository>();
        _ = services.AddScoped<IUserReadRepository, UserReadRepository>();
        _ = services.AddScoped<IWorkoutReadRepository, WorkoutReadRepository>();
        _ = services.AddScoped<ISetReadRepository, SetReadRepository>();
        _ = services.AddScoped<IExerciseReadRepository, ExerciseReadRepository>();
    }

    private static void AddAuthAndTokens(IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddScoped<IPasswordHasher, PasswordHasher>();
        _ = services.AddScoped<IEmailService, EmailService>();
        _ = services.AddScoped<IGoogleOAuthService, GoogleOAuthService>();

        services.AddOptions<JwtSettings>()
            .Bind(configuration.GetSection(JwtSettings.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        _ = services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        _ = services.AddSingleton<IJwtTokenValidator, JwtTokenValidator>();
    }

    private static void AddAutoMappers(IServiceCollection services)
    {
        _ = services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(typeof(InfrastructureInjection).Assembly);
        });
    }
}
