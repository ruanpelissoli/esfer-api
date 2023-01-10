using Esfer.API.Account.Domain.Entities;
using Esfer.API.Account.Infrastructure.CurrentAuthorizedAccount;
using Esfer.API.Account.Infrastructure.Token;
using Esfer.API.Shared.Database;
using Esfer.API.Shared.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Esfer.API.Account;

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
