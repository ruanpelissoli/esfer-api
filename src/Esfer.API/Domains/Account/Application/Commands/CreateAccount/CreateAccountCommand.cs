using Esfer.API.Domains.Shared.Mediator;

namespace Esfer.API.Domains.Account.Application.Commands.CreateAccount;

public sealed record CreateAccountCommand(
    string Username,
    string Password,
    string Email) : ICommand;
