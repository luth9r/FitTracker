using AutoMapper;
using FitTracker.Infrastructure;
using Microsoft.Extensions.Logging.Abstractions;

namespace FitTrackerInfrastructure.Tests.TestDoubles;

internal static class MapperConfigurationHelper
{
    public static MapperConfiguration Create()
    {
        return new MapperConfiguration(
            cfg =>
            {
                cfg.AddMaps(typeof(InfrastructureInjection).Assembly);
            },
            NullLoggerFactory.Instance);
    }

    public static MapperConfiguration Create<TProfile>() where TProfile : Profile, new()
    {
        return new MapperConfiguration(
            cfg =>
            {
                cfg.AddProfile<TProfile>();
            },
            NullLoggerFactory.Instance);
    }
}
