using System.Diagnostics.CodeAnalysis;
using FitTracker.Infrastructure.Persistence.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitTracker.Infrastructure.Persistence.Data;

[ExcludeFromCodeCoverage]
public class FitTrackerDbContext : DbContext
{
    public FitTrackerDbContext(DbContextOptions<FitTrackerDbContext> options)
        : base(options)
    {
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all entity configurations
        _ = modelBuilder.ApplyConfigurationsFromAssembly(typeof(FitTrackerDbContext).Assembly);

        FitTrackerSeeder.SeedData(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
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

        return await base.SaveChangesAsync(cancellationToken);
    }
}
