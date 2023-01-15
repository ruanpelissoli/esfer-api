using Esfer.API.Domains.Shared.Mediator;

namespace Esfer.API.Domains.Account.Application.Commands.ConfirmAccountEmail;

public sealed record ConfirmAccountEmailCommand(Guid AccountId, string Token) : ICommand;
