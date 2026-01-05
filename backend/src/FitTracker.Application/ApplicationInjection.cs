using System.Diagnostics.CodeAnalysis;
using FitTracker.Application.Behaviors;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FitTracker.Application;

[ExcludeFromCodeCoverage]
public static class ApplicationInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        AddMediatR(services);

        AddValidators(services);

        AddAutoMappers(services);

        _ = services.AddHttpClient();

        return services;
    }

    private static void AddMediatR(IServiceCollection services)
    {
        _ = services.AddMediatR(cfg =>
        {
            _ = cfg.RegisterServicesFromAssembly(typeof(ApplicationInjection).Assembly);
            _ = cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });
    }

    private static void AddValidators(IServiceCollection services)
    {
        _ = services.AddValidatorsFromAssembly(typeof(ApplicationInjection).Assembly);
    }

    private static void AddAutoMappers(IServiceCollection services)
    {
        _ = services.AddAutoMapper(cfg => { cfg.AddMaps(typeof(ApplicationInjection).Assembly); });
    }
}
