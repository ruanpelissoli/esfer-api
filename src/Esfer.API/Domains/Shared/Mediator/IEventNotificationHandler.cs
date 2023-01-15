using MediatR;

namespace Esfer.API.Domains.Shared.Mediator;

public interface IEventNotificationHandler<in TNotification> : INotificationHandler<TNotification>
    where TNotification : IEventNotification
{
}