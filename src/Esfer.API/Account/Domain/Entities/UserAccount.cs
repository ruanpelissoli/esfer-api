using Microsoft.AspNetCore.Identity;

namespace Esfer.API.Account.Domain.Entities;

public class UserAccount : IdentityUser<Guid>
{
    protected UserAccount() { }

    public UserAccount(string username, string email)
    {
        UserName = username;
        Email = email;
    }
}
