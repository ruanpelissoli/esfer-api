using Microsoft.AspNetCore.Identity;

namespace Esfer.API.Domains.Shared.Repository;

public interface IIdentityRepository<T>
    where T : IdentityUser<Guid>
{ }
