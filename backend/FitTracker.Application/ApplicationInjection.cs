using FitTracker.Application.Behaviors;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FitTracker.Application
{
    public static class ApplicationInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            AddMediatR(services);

            AddAutoMappers(services);

            _ = services.AddHttpClient();

            return services;
        }

        private static void AddMediatR(IServiceCollection services)
        {
            _ = services.AddMediatR(cfg =>
            {
                _ = cfg.RegisterServicesFromAssembly(typeof(ApplicationInjection).Assembly);
            });

            _ = services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        }

        private static void AddAutoMappers(IServiceCollection services)
        {
            _ = services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(typeof(ApplicationInjection).Assembly);
            });
        }
    }
}
