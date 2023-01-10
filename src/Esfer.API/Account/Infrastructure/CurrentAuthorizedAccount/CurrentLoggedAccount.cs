using System.Security.Claims;

namespace Esfer.API.Account.Infrastructure.CurrentAuthorizedAccount;

public class CurrentLoggedAccount
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public bool IsEmailConfirmed { get; set; }

    public ClaimsPrincipal Principal { get; set; } = default!;
}
