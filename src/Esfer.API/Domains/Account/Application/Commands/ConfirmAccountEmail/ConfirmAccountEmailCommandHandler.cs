using Esfer.API.Domains.Account.Domain.Entities;
using Esfer.API.Domains.Shared.Domain;
using Esfer.API.Domains.Shared.Mediator;
using Microsoft.AspNetCore.Identity;

namespace Esfer.API.Domains.Account.Application.Commands.ConfirmAccountEmail;

internal sealed class ConfirmAccountEmailCommandHandler : ICommandHandler<ConfirmAccountEmailCommand>
{
    readonly UserManager<UserAccount> _userManager;
    readonly ILogger<ConfirmAccountEmailCommandHandler> _logger;

    public ConfirmAccountEmailCommandHandler(UserManager<UserAccount> userManager, ILogger<ConfirmAccountEmailCommandHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<Result> Handle(ConfirmAccountEmailCommand request, CancellationToken cancellationToken)
    {
        var account = await _userManager.FindByIdAsync(request.AccountId.ToString());

        if (account is null)
            return Result.Failure(new Error(
                "Account.AccountNotFound",
                "Account not found"));

        var result = await _userManager.ConfirmEmailAsync(account, request.Token);

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
