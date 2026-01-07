using System.Diagnostics.CodeAnalysis;
using FitTracker.Domain.Abstract.Interfaces;
using FitTracker.Infrastructure.Persistence.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FitTracker.Infrastructure.Persistence.Data;

[ExcludeFromCodeCoverage]
public class FitTrackerDbContext : DbContext
{
    private readonly OutboxSignal _outboxSignal;

    public FitTrackerDbContext(DbContextOptions<FitTrackerDbContext> options, OutboxSignal outboxSignal)
        : base(options)
    {
        _outboxSignal = outboxSignal;
    }

    public DbSet<UserEf> Users { get; set; } = null!;

    public DbSet<WorkoutEf> Workouts { get; set; } = null!;

    public DbSet<WorkoutTemplateEf> WorkoutTemplates { get; set; } = null!;

    public DbSet<WorkoutExerciseEf> WorkoutExercises { get; set; } = null!;

    public DbSet<WorkoutTemplateExerciseEf> WorkoutTemplateExercises { get; set; } = null!;

    public DbSet<SetEf> Sets { get; set; } = null!;

    public DbSet<TemplateSetEf> TemplateSets { get; set; } = null!;

    public DbSet<ExerciseEf> Exercises { get; set; } = null!;

    public DbSet<AchievementEf> Achievements { get; set; } = null!;

    public DbSet<UserAchievementEf> UserAchievements { get; set; } = null!;

    public DbSet<ExerciseRecordEf> ExerciseRecords { get; set; } = null!;

    public DbSet<OutboxMessage> OutboxMessages { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all entity configurations
        _ = modelBuilder.ApplyConfigurationsFromAssembly(typeof(FitTrackerDbContext).Assembly);

        if (Database.IsNpgsql())
        {
            FitTrackerSeeder.SeedData(modelBuilder);
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditLog();

        ProcessDomainEvents();

        var result = await base.SaveChangesAsync(cancellationToken);

        if (ChangeTracker.Entries<OutboxMessage>().Any(e => e.State == EntityState.Added))
        {
            // Trigger a signal to send outbox messages
            _outboxSignal.Writer.TryWrite(true);
        }

        return result;
    }

    private void UpdateAuditLog()
    {
        // Automatically set UpdatedAt for modified entities
        foreach (var entry in ChangeTracker.Entries<BaseEntityEf>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }
    }

    private void ProcessDomainEvents()
    {
        // Process all domain events in the current context
        var domainEntities = ChangeTracker.Entries<IHasDomainEvents>()
            .Where(x => x.Entity.DomainEvents.Any())
            .Select(x => x.Entity)
            .ToList();

        var outboxMessages = domainEntities
            .SelectMany(entity =>
            {
                var events = entity.DomainEvents.ToList();
                entity.ClearDomainEvents(); // Clear events so they don't get processed twice
                return events;
            })
            .Select(domainEvent => new OutboxMessage
            {
                Id = Guid.NewGuid(),
                OccurredOnUtc = DateTime.UtcNow,
                Type = domainEvent.GetType().Name,
                Content = JsonConvert.SerializeObject(
                    domainEvent,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All,
                    }),
            })
            .ToList();

        if (outboxMessages.Any())
        {
            OutboxMessages.AddRange(outboxMessages);
        }
    }
}
