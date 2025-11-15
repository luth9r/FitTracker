using FitTracker.Application.Interfaces;
using FitTracker.Application.UseCases.User.Commands;
using FitTracker.Domain.Abstract.Interfaces;
using FitTracker.Infrastructure.Localization;
using FitTracker.Infrastructure.Persistence.Data;
using FitTracker.Infrastructure.Persistence.Repositories;
using FitTracker.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FitTracker.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // ============================================
        // Database Configuration
        // ============================================
        AddDatabase(services, configuration);

        // ============================================
        // Localization Services
        // ============================================
        AddLocalization(services);

        // ============================================
        // Repository Registration
        // ============================================
        AddRepositories(services);

        // ============================================
        // Automappers
        // ============================================
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

        services.AddDbContext<FitTrackerDbContext>(options =>
        {
            options.UseNpgsql(
                connectionString,
                npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsAssembly("FitTracker.Infrastructure");
                    npgsqlOptions.CommandTimeout(30);
                    npgsqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorCodesToAdd: null
                    );

                }
            );

            var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

            if (isDevelopment)
            {
                options.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            }
        });
    }

    private static void AddLocalization(IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddSingleton<JsonLocalizationProvider>();

        services.AddScoped<ILocalizationService, LocalizationService>();
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IGoogleTokenValidator, GoogleTokenValidator>();
        // TODO:
        // services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
        // services.AddScoped<IUnitOfWork, UnitOfWork>();
        // services.AddScoped<IWorkoutRepository, WorkoutRepository>();
        // services.AddScoped<IExerciseRepository, ExerciseRepository>();
    }

    private static void AddAutoMappers(IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(typeof(DependencyInjection).Assembly);
        });
    }
}
