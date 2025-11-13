using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FitTracker.Application
{
    public static class ApplicationInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            AddAutoMappers(services);

            return services;
        }

        private static void AddAutoMappers(IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(typeof(Mapping.RegisterProfile).Assembly);
            });
        }
    }
}
