using Esfer.API.Shared.Mediator;

namespace Esfer.API.Account.Application.Login;

public sealed record LoginCommand(
    string Username,
    string Password) : ICommand<LoginResponse>;
