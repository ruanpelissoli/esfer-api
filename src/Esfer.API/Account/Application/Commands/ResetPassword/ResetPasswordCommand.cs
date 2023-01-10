using Esfer.API.Shared.Mediator;

namespace Esfer.API.Account.Application.Commands.ResetPassword;

public sealed record ResetPasswordCommand(string Email) : ICommand;
