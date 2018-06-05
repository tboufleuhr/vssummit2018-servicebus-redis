using MediatR;

namespace VsSummit2018.Domain
{
    public interface IEventHandler<TEvent> : INotificationHandler<TEvent>
        where TEvent : Event
    {
    }
}
