using Esfer.API.Domains.Shared.Mediator;

namespace Esfer.API.Domains.Account.Application.Commands.Login;

public sealed record LoginCommand(
    string UserName,
    string Password) : ICommand<LoginResponse>;
