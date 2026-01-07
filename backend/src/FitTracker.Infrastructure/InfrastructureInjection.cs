using System.Diagnostics.CodeAnalysis;
using FitTracker.Application.Interfaces;
using FitTracker.Domain.Abstract.Interfaces;
using FitTracker.Infrastructure.BackgroundJobs;
using FitTracker.Infrastructure.Localization;
using FitTracker.Infrastructure.Messaging.Consumers;
using FitTracker.Infrastructure.Persistence;
using FitTracker.Infrastructure.Persistence.Data;
using FitTracker.Infrastructure.Persistence.Repositories;
using FitTracker.Infrastructure.Services;
using FitTracker.Infrastructure.Settings;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace FitTracker.Infrastructure;

[ExcludeFromCodeCoverage]
public static class InfrastructureInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        AddDatabase(services, configuration);

        AddRedis(services);

        AddRebbitMq(services);

        AddSignals(services);

        AddLocalization(services);

        AddRepositories(services);

        AddHostedServices(services);

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
            throw new InvalidOperationException("Database connection string 'DefaultConnection' is not configured.");
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
                        3,
                        TimeSpan.FromSeconds(30),
                        null);
                });

            var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

            if (isDevelopment)
            {
                _ = options.LogTo(Console.WriteLine, LogLevel.Information)
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

    private static void AddHostedServices(IServiceCollection services)
    {
        services.AddHostedService<OutboxProcessor>();
        services.AddHostedService<OutboxCleaner>();
        services.AddHostedService<UnverifiedUserCleaner>();
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
        _ = services.AddAutoMapper(cfg => { cfg.AddMaps(typeof(InfrastructureInjection).Assembly); });
    }

    private static void AddRedis(IServiceCollection services)
    {
        services.AddSingleton<IConnectionMultiplexer>(sp =>
            ConnectionMultiplexer.Connect("localhost:6379"));
        services.AddScoped<IRateLimitService, RedisRateLimitService>();
    }

    private static void AddRebbitMq(IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<UserVerificationRequestedConsumer>();
            x.AddConsumer<UserRegisteredConsumer>();
            x.AddConsumer<UserRequestedPasswordResetConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost");

                cfg.ReceiveEndpoint(
                    "user-verification-queue",
                    e =>
                    {
                        e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));

                        e.PrefetchCount = 16;

                        e.ConfigureConsumer<UserVerificationRequestedConsumer>(context);
                    });

                cfg.ReceiveEndpoint(
                    "user-registered-queue",
                    e =>
                    {
                        e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));

                        e.PrefetchCount = 16;

                        e.ConfigureConsumer<UserRegisteredConsumer>(context);
                    });

                cfg.ReceiveEndpoint(
                    "user-requested-password-reset-queue",
                    e =>
                    {
                        e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));

                        e.PrefetchCount = 16;

                        e.ConfigureConsumer<UserRequestedPasswordResetConsumer>(context);
                    });
            });
        });
    }

    private static void AddSignals(IServiceCollection services)
    {
        services.AddSingleton<OutboxSignal>();
    }
}
