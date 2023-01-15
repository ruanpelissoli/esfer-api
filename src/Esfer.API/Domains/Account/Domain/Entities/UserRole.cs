using Microsoft.AspNetCore.Identity;

namespace Esfer.API.Domains.Account.Domain.Entities;

public class UserRole : IdentityRole<Guid>
{
    public UserRole() : base() { }

    public UserRole(string name, string description) : base(name)
    {
        Description = description;
    }
    public virtual string Description { get; set; } = string.Empty;
    public override Guid Id { get; set; }
}
