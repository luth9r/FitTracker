using AutoMapper;
using FitTracker.Infrastructure;
using FitTracker.Infrastructure.Persistence;
using FitTracker.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace FitTrackerInfrastructure.Tests.Repositories;

public class RepositoryTestBase : IDisposable
{
    protected readonly FitTrackerDbContext context;
    protected readonly IMapper mapper;

    protected RepositoryTestBase()
    {
        var options = new DbContextOptionsBuilder<FitTrackerDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .EnableSensitiveDataLogging()
            .Options;

        context = new FitTrackerDbContext(options, new OutboxSignal());

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddMaps(typeof(InfrastructureInjection).Assembly);
        }, NullLoggerFactory.Instance);

        mapper = config.CreateMapper();

        context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        context.Database.EnsureDeleted();
        context.Dispose();

        GC.SuppressFinalize(this);
    }
}
