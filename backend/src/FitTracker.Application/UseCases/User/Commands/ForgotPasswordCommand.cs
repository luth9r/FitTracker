using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using MediatR;

namespace FitTracker.Application.UseCases.User.Commands;

/// <summary>
///     Represents a command to initiate the forgot password process for a user.
/// </summary>
/// <param name="Email">The email address of the user.</param>
[ExcludeFromCodeCoverage]
public sealed record ForgotPasswordCommand(string Email) : IRequest<Result>;
