using Esfer.API.Shared.Mediator;

namespace Esfer.API.Account.Application.Events.SendConfirmationAccountEmail;

public sealed record SendConfirmationAccountEmailNotification(string Email)
    : IEventNotification;
