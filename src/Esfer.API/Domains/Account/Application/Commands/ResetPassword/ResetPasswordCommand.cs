using Esfer.API.Domains.Shared.Mediator;

namespace Esfer.API.Domains.Account.Application.Commands.ResetPassword;

public sealed record ResetPasswordCommand(string Email) : ICommand;
