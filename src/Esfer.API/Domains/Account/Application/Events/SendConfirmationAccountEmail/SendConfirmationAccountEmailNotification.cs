using Esfer.API.Domains.Shared.Mediator;

namespace Esfer.API.Domains.Account.Application.Events.SendConfirmationAccountEmail;

public sealed record SendConfirmationAccountEmailNotification(string Email)
    : IEventNotification;
