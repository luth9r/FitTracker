using AutoMapper;
using CSharpFunctionalExtensions;
using FitTracker.Domain.Abstract.Interfaces;
using MediatR;

namespace FitTracker.Application.Features.WorkoutTemplate.Queries.GetWorkoutTemplate;

/// <summary>
///     Handles the <see cref="GetTemplateWorkoutQuery" /> to retrieve a workout template by its ID
///     and the user ID associated with it. Returns the template if found as a result object.
/// </summary>
/// <param name="repository">The workout template read repository.</param>
public sealed class GetTemplateWorkoutQueryHandler(
    IWorkoutTemplateReadRepository repository,
    IMapper mapper)
    : IRequestHandler<GetTemplateWorkoutQuery, Result<GetTemplateWorkoutResponses>>
{
    /// <summary>
    ///     Handles the processing of the <see cref="GetTemplateWorkoutQuery" /> to retrieve a workout template
    ///     by its template ID and associated user ID, returning the corresponding response if successful.
    /// </summary>
    /// <param name="request">The query request containing the template ID and user ID.</param>
    /// <param name="cancellationToken">The cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>
    ///     A result containing the <see cref="GetTemplateWorkoutResponses" /> if the template is found,
    ///     or a failure result if it cannot be located.
    /// </returns>
    public async Task<Result<GetTemplateWorkoutResponses>> Handle(
        GetTemplateWorkoutQuery request,
        CancellationToken cancellationToken)
    {
        var template = await repository.GetTemplateByIdAsync(
            request.TemplateId,
            request.UserId,
            cancellationToken);

        if (template is null)
        {
            return Result.Failure<GetTemplateWorkoutResponses>("Template was not found.");
        }

        var response = mapper.Map<GetTemplateWorkoutResponses>(template);

        return Result.Success(response);
    }
}
