using Esfer.API.Shared.Mediator;

namespace Esfer.API.Account.Application.CreateAccount;

public sealed record CreateAccountCommand(
    string Username,
    string Password,
    string Email) : ICommand;
