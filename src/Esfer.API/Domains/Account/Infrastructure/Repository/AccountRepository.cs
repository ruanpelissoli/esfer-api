using Esfer.API.Domains.Account.Domain.Entities;
using Esfer.API.Domains.Account.Domain.Repository;
using Microsoft.AspNetCore.Identity;

namespace Esfer.API.Domains.Account.Infrastructure.Repository;

internal class AccountRepository : IAccountRepository
{
    readonly UserManager<UserAccount> _userManager;

    public AccountRepository(UserManager<UserAccount> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IdentityResult> CreateAsync(string userName, string email, string password)
        => await _userManager.CreateAsync(new(userName, email), password);

    public async Task<UserAccount?> FindByEmailAsync(string email)
        => await _userManager.FindByEmailAsync(email);

    public async Task<UserAccount?> FindByIdAsync(Guid id)
        => await _userManager.FindByIdAsync(id.ToString());

    public async Task<UserAccount?> FindByNameAsync(string userName)
        => await _userManager.FindByNameAsync(userName);

    public async Task<bool> CheckPasswordAsync(UserAccount account, string password)
        => await _userManager.CheckPasswordAsync(account, password);

    public async Task<IdentityResult> ConfirmEmailAsync(UserAccount account, string token)
        => await _userManager.ConfirmEmailAsync(account, token);

    public async Task<string> GenerateEmailConfirmationTokenAsync(UserAccount account)
        => await _userManager.GenerateEmailConfirmationTokenAsync(account);

    public async Task<string> GeneratePasswordResetTokenAsync(UserAccount account)
        => await _userManager.GeneratePasswordResetTokenAsync(account);

    public async Task<bool> IsEmailConfirmedAsync(UserAccount account)
        => await _userManager.IsEmailConfirmedAsync(account);

    public async Task<IdentityResult> ResetPasswordAsync(UserAccount account, string token, string newPassword)
        => await _userManager.ResetPasswordAsync(account, token, newPassword);
}