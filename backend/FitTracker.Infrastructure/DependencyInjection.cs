using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitTracker.Infrastructure.Persistence.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace FitTracker.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // DbContext Registration
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<FitTrackerDbContext>(options =>
            {
                options.UseNpgsql(
                    connectionString,
                    sqlServerOptions =>
                    {
                        sqlServerOptions.MigrationsAssembly("FitTracker.Infrastructure");
                        sqlServerOptions.CommandTimeout(30);
                        sqlServerOptions.EnableRetryOnFailure(
                            maxRetryCount: 3,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorCodesToAdd: null
                        );
                    }
                );

                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                {
                    options.LogTo(Console.WriteLine)
                        .EnableSensitiveDataLogging();
                }
            });

            // Repository Registration
            //services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
            //services.AddScoped<IUnitOfWork, UnitOfWork>();
            //services.AddScoped<IUserRepository, UserRepository>();
            //services.AddScoped<IWorkoutRepository, WorkoutRepository>();
            //services.AddScoped<IExerciseRepository, ExerciseRepository>();

            return services;
        }
    }
}
