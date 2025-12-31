using CSharpFunctionalExtensions;
using MediatR;

namespace FitTracker.Application.UseCases.User.Commands
{
    public sealed record ForgotPasswordCommand(string Email) : IRequest<Result>;
}
