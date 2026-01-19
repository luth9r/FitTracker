using System.Diagnostics.CodeAnalysis;
using FitTracker.Domain.Enums;

namespace FitTracker.Domain.Entities.TemplateAggregate;

/// <summary>
///     Represents data for a set in a workout template. This record stores information about the set including
///     its number, weight, repetitions, rest interval, and type.
/// </summary>
[ExcludeFromCodeCoverage]
public record TemplateSetData(
    int SetNumber,
    double Weight,
    int Reps,
    int? Rest,
    SetType Type);
