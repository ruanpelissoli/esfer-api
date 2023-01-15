using Esfer.API.Domains.Account.Domain.Entities;
using Esfer.API.Domains.Games.Domain.Entities;
using Esfer.API.Domains.Shared.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Esfer.API.Domains.Shared.Database;

public class EsferDbContext : IdentityDbContext<UserAccount, UserRole, Guid>
{
    public DbSet<Game> Games { get; set; }

    public EsferDbContext(DbContextOptions<EsferDbContext> options)
    : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().
                                GetProperty(nameof(Entity.CreatedAt)) != null))
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(nameof(Entity.CreatedAt)).CurrentValue = DateTime.UtcNow;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Property(nameof(Entity.UpdatedAt)).CurrentValue = DateTime.UtcNow;
            }
        }

        return (await base.SaveChangesAsync(true, cancellationToken));
    }
}
