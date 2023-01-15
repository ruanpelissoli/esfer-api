using Esfer.API.Domains.Shared.Mediator;

namespace Esfer.API.Domains.Account.Application.Commands.ChangePassword;

public sealed record ChangePasswordCommand(Guid AccountId, string Token, string NewPassword) : ICommand;
