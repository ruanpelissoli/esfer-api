using Microsoft.AspNetCore.Authorization;

namespace Esfer.API.Domains.Account.Infrastructure.CurrentAuthorizedAccount;

public class AuthorizationRequirementsHandler : AuthorizationHandler<CheckCurrentLoggedAccountRequirement>
{
    private readonly CurrentLoggedAccount _loggedAccount;

    public AuthorizationRequirementsHandler(CurrentLoggedAccount loggedAccount)
        => _loggedAccount = loggedAccount;

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CheckCurrentLoggedAccountRequirement requirement)
    {
        if (_loggedAccount.UserName is not null && _loggedAccount.IsEmailConfirmed)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
