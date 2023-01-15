using Esfer.API.Domains.Account.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Esfer.API.Domains.Account.Infrastructure.CurrentAuthorizedAccount;

public class ClaimsTransformation : IClaimsTransformation
{
    readonly UserManager<UserAccount> _userManager;
    readonly CurrentLoggedAccount _currentLoggedAccount;

    public ClaimsTransformation(UserManager<UserAccount> userManager, CurrentLoggedAccount currentLoggedAccount)
    {
        _userManager = userManager;
        _currentLoggedAccount = currentLoggedAccount;
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        _currentLoggedAccount.Principal = principal;

        if (principal.FindFirstValue(ClaimTypes.Name) is { Length: > 0 } name)
        {
            var account = await _userManager.FindByNameAsync(name);

            if (account != null)
            {
                _currentLoggedAccount.Id = account.Id;
                _currentLoggedAccount.UserName = account.UserName!;
                _currentLoggedAccount.Email = account.Email!;
                _currentLoggedAccount.IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(account);
            }
        }

        return principal;
    }
}
