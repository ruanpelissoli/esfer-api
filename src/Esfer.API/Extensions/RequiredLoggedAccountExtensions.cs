using Esfer.API.Domains.Account.Infrastructure.CurrentAuthorizedAccount;

namespace Esfer.API.Extensions;

public static class RequiredLoggedAccountExtensions
{
    public static RouteHandlerBuilder RequiredLoggedAccount(this RouteHandlerBuilder builder)
    {
        return builder.RequireAuthorization(policy =>
            policy
              .RequireAuthenticatedUser()
              .AddRequirements(new CheckCurrentLoggedAccountRequirement()));
    }
}
