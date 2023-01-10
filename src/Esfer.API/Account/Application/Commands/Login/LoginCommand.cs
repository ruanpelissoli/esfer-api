using Esfer.API.Shared.Mediator;

namespace Esfer.API.Account.Application.Commands.Login;

public sealed record LoginCommand(
    string UserName,
    string Password) : ICommand<LoginResponse>;
