using Esfer.API.Domains.Shared.Mediator;

namespace Esfer.API.Domains.Account.Application.Commands.CreateAccount;

public sealed record CreateAccountCommand(
    string UserName,
    string Password,
    string Email) : ICommand;
