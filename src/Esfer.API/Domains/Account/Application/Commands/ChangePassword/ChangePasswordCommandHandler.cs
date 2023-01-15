using Esfer.API.Domains.Account.Domain.Entities;
using Esfer.API.Domains.Shared.Domain;
using Esfer.API.Domains.Shared.Mediator;
using Microsoft.AspNetCore.Identity;

namespace Esfer.API.Domains.Account.Application.Commands.ChangePassword;

internal sealed class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand>
{
    readonly UserManager<UserAccount> _userManager;
    readonly ILogger<ChangePasswordCommandHandler> _logger;

    public ChangePasswordCommandHandler(UserManager<UserAccount> userManager, ILogger<ChangePasswordCommandHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<Result> Handle(ChangePasswordCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Trying to change password for account {AccountId}", command.AccountId);

        var account = await _userManager.FindByIdAsync(command.AccountId.ToString());

        if (account == null)
            return Result.Failure(new Error("" +
                "Account.NotFound",
                "Account not found"));

        var result = await _userManager.ResetPasswordAsync(account, command.Token, command.NewPassword);

        if (!result.Succeeded)
            return Result.Failure(new Error("" +
                "Account.PasswordChange",
                string.Join(",", result.Errors.Select(s => s.Description))));

        return Result.Success();
    }
}
