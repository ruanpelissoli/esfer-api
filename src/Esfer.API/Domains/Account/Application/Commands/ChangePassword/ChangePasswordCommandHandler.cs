using Esfer.API.Domains.Account.Domain.Repository;
using Esfer.API.Domains.Shared.Domain;
using Esfer.API.Domains.Shared.Mediator;

namespace Esfer.API.Domains.Account.Application.Commands.ChangePassword;

internal sealed class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand>
{
    readonly IAccountRepository _accountRepository;
    readonly ILogger<ChangePasswordCommandHandler> _logger;

    public ChangePasswordCommandHandler(
        IAccountRepository accountRepository,
        ILogger<ChangePasswordCommandHandler> logger)
    {
        _accountRepository = accountRepository;
        _logger = logger;
    }

    public async Task<Result> Handle(ChangePasswordCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Trying to change password for account {AccountId}", command.AccountId);

        var account = await _accountRepository.FindByIdAsync(command.AccountId);

        if (account == null)
            return Result.Failure(new Error("" +
                "Account.NotFound",
                "Account not found"));

        var result = await _accountRepository.ResetPasswordAsync(account, command.Token, command.NewPassword);

        if (!result.Succeeded)
            return Result.Failure(new Error("" +
                "Account.PasswordChange",
                string.Join(",", result.Errors.Select(s => s.Description))));

        return Result.Success();
    }
}
