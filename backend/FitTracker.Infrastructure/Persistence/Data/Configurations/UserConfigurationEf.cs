using FitTracker.Domain.Entities;
using FitTracker.Domain.ValueObjects;
using FitTracker.Infrastructure.Persistence.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitTracker.Infrastructure.Persistence.Data.Configurations
{
    public class UserConfigurationEf : IEntityTypeConfiguration<UserEf>
    {
        public void Configure(EntityTypeBuilder<UserEf> builder)
        {
            _ = builder.ToTable("users");

            _ = builder.HasKey(u => u.Id);

            _ = builder.Property(u => u.Id)
                .HasColumnName("id")
                .HasColumnType("uuid");

            _ = builder.Property(u => u.Username)
                .HasColumnName("username")
                .HasMaxLength(User.UsernameMaxLength)
                .IsRequired();

            _ = builder.Property(u => u.Email)
                .HasColumnName("email")
                .HasMaxLength(User.EmailMaxLength)
                .IsRequired();

            _ = builder.Property(u => u.PasswordHash)
                .HasColumnName("password_hash")
                .HasMaxLength(255);

            _ = builder.Property(u => u.FirstName)
                .HasColumnName("first_name")
                .HasMaxLength(User.FirstNameMaxLength);

            _ = builder.Property(u => u.LastName)
                .HasColumnName("last_name")
                .HasMaxLength(User.LastNameMaxLength);

            _ = builder.Property(u => u.Avatar)
                .HasColumnName("avatar")
                .HasMaxLength(500);

            _ = builder.Property(u => u.Bio)
                .HasColumnName("bio")
                .HasMaxLength(User.BioMaxLength);

            _ = builder.Property<string>("PreferredUnits")
                .HasColumnName("preferred_units")
                .HasMaxLength(10)
                .IsRequired()
                .HasDefaultValue("metric");

            _ = builder.Property(u => u.IsEmailVerified)
                .HasColumnName("is_email_verified")
                .HasDefaultValue(false)
                .IsRequired();

            _ = builder.Property(u => u.GoogleProviderId)
                .HasColumnName("google_provider_id")
                .HasMaxLength(255);

            _ = builder.Property(u => u.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

            _ = builder.Property(u => u.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

            // Indexes
            _ = builder.HasIndex(u => u.Username)
                .IsUnique()
                .HasDatabaseName("IX_Users_Username");

            _ = builder.HasIndex(u => u.Email)
                .IsUnique()
                .HasDatabaseName("IX_Users_Email");

            // Relationships
            _ = builder.HasMany(u => u.Workouts)
                .WithOne(w => w.User)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            _ = builder.HasMany(u => u.WorkoutTemplates)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            _ = builder.HasMany(u => u.CustomExercises)
                .WithOne(e => e.CreatedByUser)
                .HasForeignKey(e => e.CreatedByUserId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
