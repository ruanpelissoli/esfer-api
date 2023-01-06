using Esfer.API.Account.Domain.Entities;
using Esfer.API.Shared.Domain;
using Esfer.API.Shared.Mediator;
using Microsoft.AspNetCore.Identity;

namespace Esfer.API.Account.Application.CreateAccount;

internal sealed class CreateAccountCommandHandler
    : ICommandHandler<CreateAccountCommand>
{
    readonly UserManager<UserAccount> _userManager;
    readonly ILogger<CreateAccountCommandHandler> _logger;

    public CreateAccountCommandHandler(
        UserManager<UserAccount> userManager,
        ILogger<CreateAccountCommandHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<Result> Handle(CreateAccountCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting account creation for {0} and {1}",
            command.Username, command.Email);

        var result = await _userManager.CreateAsync(
           new(command.Username, command.Email),
           command.Password);

        if (!result.Succeeded)
        {
            _logger.LogInformation("Account create failed for {0} and {1}",
            command.Username, command.Email);

            return Result.Failure(new Error(
                "Account.CreateFailed",
                string.Join(",", result.Errors.Select(s => s.Description))));
        }

        // TODO: SendEmails
        // _eventSender.SendAndForget(new SendAccountConfimationEmailEvent(request.Email), cancellationToken);

        return Result.Success();
    }
}
