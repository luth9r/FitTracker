using Microsoft.AspNetCore.Authorization;

namespace FitTracker.Api.Authorization
{
    public class EmailVerifiedHandler : AuthorizationHandler<EmailVerifiedRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            EmailVerifiedRequirement requirement)
        {
            var emailVerifiedClaim = context.User.FindFirst("is_email_verified");

            if (emailVerifiedClaim != null && emailVerifiedClaim.Value.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
