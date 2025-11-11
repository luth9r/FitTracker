using System;
using FitTracker.Domain.Entities;

namespace FitTracker.Tests.Factories
{
    public static class WorkoutTemplateFactory
    {
        public static WorkoutTemplate Create(
            Guid? userId = null,
            string? name = null,
            string? description = null,
            int usageCount = 0,
            DateTime? lastUsedAt = null)
        {
            return new WorkoutTemplate(
                userId ?? Guid.NewGuid(),
                name ?? "Default Workout Name",
                description,
                usageCount,
                lastUsedAt);
        }
    }
}
