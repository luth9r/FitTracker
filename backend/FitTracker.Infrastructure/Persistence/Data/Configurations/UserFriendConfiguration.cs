using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FitTracker.Infrastructure.Persistence.Data.Configurations
{
    public class UserFriendConfiguration : IEntityTypeConfiguration<UserFriend>
    {
        public void Configure(EntityTypeBuilder<UserFriend> builder)
        {
            builder.HasKey(uf => uf.Id);

            builder.Property(uf => uf.Status)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("pending");

            builder.Property(uf => uf.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

            builder.Property(uf => uf.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

            // Indexes
            builder.HasIndex(uf => new { uf.UserId, uf.FriendId })
                .HasDatabaseName("IX_UserFriend_User_Friend")
                .IsUnique(); // Prevent duplicate friend requests

            builder.HasIndex(uf => uf.Status)
                .HasDatabaseName("IX_UserFriend_Status");

            // Relationships
            builder.HasOne(uf => uf.User)
                .WithMany(u => u.Friends)
                .HasForeignKey(uf => uf.UserId)
                .HasConstraintName("FK_UserFriend_User_UserId")
                .OnDelete(DeleteBehavior.Cascade);

            // "FriendOf" (Friend -> UserFriend)
            builder.HasOne(uf => uf.Friend)
                .WithMany(u => u.FriendOf)
                .HasForeignKey(uf => uf.FriendId)
                .HasConstraintName("FK_UserFriend_Friend_FriendId")
                .OnDelete(DeleteBehavior.NoAction); // Prevent cascading on both sides
        }
    }
}
