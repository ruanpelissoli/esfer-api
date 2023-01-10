using Esfer.API.Shared.Mediator;

namespace Esfer.API.Account.Application.Commands.ChangePassword;

public sealed record ChangePasswordCommand(Guid AccountId, string Token, string NewPassword) : ICommand;
