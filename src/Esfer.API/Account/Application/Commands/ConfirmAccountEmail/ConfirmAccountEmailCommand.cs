using Esfer.API.Shared.Mediator;

namespace Esfer.API.Account.Application.Commands.ConfirmAccountEmail;

public sealed record ConfirmAccountEmailCommand(Guid AccountId, string Token) : ICommand;
