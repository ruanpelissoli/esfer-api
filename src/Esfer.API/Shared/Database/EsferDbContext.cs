using Esfer.API.Account.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Esfer.API.Shared.Database;

public class EsferDbContext : IdentityDbContext<UserAccount, UserRole, Guid>
{
    public EsferDbContext(DbContextOptions<EsferDbContext> options)
    : base(options)

    { }
}
