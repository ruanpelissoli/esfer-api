using Esfer.API.Domains.Account.Domain.Entities;
using Esfer.API.Domains.Account.Infrastructure.CurrentAuthorizedAccount;
using Esfer.API.Domains.Account.Infrastructure.Token;
using Esfer.API.Domains.Shared.Database;
using Esfer.API.Domains.Shared.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Esfer.API.Domains.Account;

public class AccountInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ITokenService, TokenService>();

        services.AddScoped<CurrentLoggedAccount>();
        services.AddScoped<IClaimsTransformation, ClaimsTransformation>();
        services.AddScoped<IAuthorizationHandler, AuthorizationRequirementsHandler>();

        services.AddIdentity<UserAccount, UserRole>(options =>
        {
            options.Password.RequiredLength = 8;
            options.Password.RequireDigit = false;
            options.Password.RequireUppercase = false;

            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = true;
        })
        .AddEntityFrameworkStores<EsferDbContext>()
        .AddDefaultTokenProviders();
    }
}
