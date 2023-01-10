using Esfer.API.Shared.Mediator;

namespace Esfer.API.Account.Application.Commands.CreateAccount;

public sealed record CreateAccountCommand(
    string Username,
    string Password,
    string Email) : ICommand;
