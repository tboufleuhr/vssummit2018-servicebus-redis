using System.Threading.Tasks;
using VsSummit2018.Domain;

namespace VsSummit2018.Infra.MessageBroker
{
    public interface IEventSubscriberFactory
    {
        Task<EventSubscriber<TEvent>> CreateSubscriberAsync<TEvent>(string topic, IEventHandler<TEvent> eventHandler)
            where TEvent : Event;
    }
}
