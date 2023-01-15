using Esfer.API.Domains.Account.Application.Events.SendConfirmationAccountEmail;
using Esfer.API.Domains.Account.Domain.Entities;
using Esfer.API.Domains.Shared.Domain;
using Esfer.API.Domains.Shared.Mediator;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Esfer.API.Domains.Account.Application.Commands.CreateAccount;

internal sealed class CreateAccountCommandHandler
    : ICommandHandler<CreateAccountCommand>
{
    readonly UserManager<UserAccount> _userManager;
    readonly IPublisher _publisher;
    readonly ILogger<CreateAccountCommandHandler> _logger;

    public CreateAccountCommandHandler(
        UserManager<UserAccount> userManager,
        IPublisher publisher,
        ILogger<CreateAccountCommandHandler> logger)
    {
        _userManager = userManager;
        _publisher = publisher;
        _logger = logger;
    }

    public async Task<Result> Handle(CreateAccountCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting account creation for {Username} and {Email}",
            command.Username, command.Email);

        var result = await _userManager.CreateAsync(
           new(command.Username, command.Email),
           command.Password);

        if (!result.Succeeded)
        {
            _logger.LogInformation("Account create failed for {Username} and {Email}",
            command.Username, command.Email);

            return Result.Failure(new Error(
                "Account.CreateFailed",
                string.Join(",", result.Errors.Select(s => s.Description))));
        }

        await _publisher.Publish(new SendConfirmationAccountEmailNotification(command.Email),
            cancellationToken);

        return Result.Success();
    }
}
