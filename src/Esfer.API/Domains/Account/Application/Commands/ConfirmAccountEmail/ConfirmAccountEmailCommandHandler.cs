using Esfer.API.Domains.Account.Domain.Repository;
using Esfer.API.Domains.Shared.Domain;
using Esfer.API.Domains.Shared.Mediator;

namespace Esfer.API.Domains.Account.Application.Commands.ConfirmAccountEmail;

internal sealed class ConfirmAccountEmailCommandHandler : ICommandHandler<ConfirmAccountEmailCommand>
{
    readonly IAccountRepository _accountRepository;
    readonly ILogger<ConfirmAccountEmailCommandHandler> _logger;

    public ConfirmAccountEmailCommandHandler(
        IAccountRepository accountRepository,
        ILogger<ConfirmAccountEmailCommandHandler> logger)
    {
        _accountRepository = accountRepository;
        _logger = logger;
    }

    public async Task<Result> Handle(ConfirmAccountEmailCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.FindByIdAsync(request.AccountId);

        if (account is null)
            return Result.Failure(new Error(
                "Account.AccountNotFound",
                "Account not found"));

        var result = await _accountRepository.ConfirmEmailAsync(account, request.Token);

        if (result.Succeeded)
        {
            _logger.LogInformation("Email for user {0} is confirmed", account.UserName);
            return Result.Success();
        }

        return Result.Failure(new Error(
                "Account.FailToConfirmEmail",
                string.Join(",", result.Errors.Select(s => s.Description))));
    }
}
