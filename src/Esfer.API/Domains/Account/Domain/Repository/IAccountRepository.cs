using Esfer.API.Domains.Account.Domain.Entities;
using Esfer.API.Domains.Shared.Repository;
using Microsoft.AspNetCore.Identity;

namespace Esfer.API.Domains.Account.Domain.Repository;

public interface IAccountRepository : IIdentityRepository<UserAccount>
{
    Task<IdentityResult> CreateAsync(string userName, string Email, string password);
    Task<UserAccount?> FindByEmailAsync(string email);
    Task<UserAccount?> FindByIdAsync(Guid id);
    Task<UserAccount?> FindByNameAsync(string userName);
    Task<bool> CheckPasswordAsync(UserAccount account, string password);
    Task<bool> IsEmailConfirmedAsync(UserAccount account);
    Task<IdentityResult> ConfirmEmailAsync(UserAccount account, string token);
    Task<IdentityResult> ResetPasswordAsync(UserAccount account, string token, string newPassword);
    Task<string> GeneratePasswordResetTokenAsync(UserAccount account);
    Task<string> GenerateEmailConfirmationTokenAsync(UserAccount account);
}
