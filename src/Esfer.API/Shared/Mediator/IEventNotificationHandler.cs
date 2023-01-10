using MediatR;

namespace Esfer.API.Shared.Mediator;

public interface IEventNotificationHandler<in TNotification> : INotificationHandler<TNotification>
    where TNotification : IEventNotification
{
}