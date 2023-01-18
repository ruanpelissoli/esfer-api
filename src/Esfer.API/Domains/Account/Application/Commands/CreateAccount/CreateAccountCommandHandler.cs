using Esfer.API.Domains.Account.Application.Events.SendConfirmationAccountEmail;
using Esfer.API.Domains.Account.Domain.Errors;
using Esfer.API.Domains.Account.Domain.Repository;
using Esfer.API.Domains.Shared.Domain;
using Esfer.API.Domains.Shared.Mediator;
using MediatR;

namespace Esfer.API.Domains.Account.Application.Commands.CreateAccount;

public sealed class CreateAccountCommandHandler
    : ICommandHandler<CreateAccountCommand>
{
    readonly IAccountRepository _accountRepository;
    readonly IPublisher _publisher;
    readonly ILogger<CreateAccountCommandHandler> _logger;

    public CreateAccountCommandHandler(
        IAccountRepository accountRepository,
        IPublisher publisher,
        ILogger<CreateAccountCommandHandler> logger)
    {
        _accountRepository = accountRepository;
        _publisher = publisher;
        _logger = logger;
    }

    public async Task<Result> Handle(CreateAccountCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting account creation for {Username} and {Email}",
            command.UserName, command.Email);

        var result = await _accountRepository.CreateAsync(
            command.UserName,
            command.Email,
            command.Password);

        if (!result.Succeeded)
        {
            _logger.LogInformation("Account create failed for {Username} and {Email}",
            command.UserName, command.Email);

            return Result.Failure(AccountErrors.CreateCreationFailed(result.Errors));
        }

        await _publisher.Publish(new SendConfirmationAccountEmailNotification(command.Email),
            cancellationToken);

        return Result.Success();
    }
}
