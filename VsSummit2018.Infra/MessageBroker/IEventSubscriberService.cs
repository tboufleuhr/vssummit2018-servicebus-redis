using System.Threading.Tasks;
using VsSummit2018.Domain;

namespace VsSummit2018.Infra.MessageBroker
{
    public interface IEventSubscriberService
    {
        Task AddSubscriberAsync<TEvent>(EventSubscriber<TEvent> eventSubscriber) where TEvent : Event;
        Task PublishAsync<TEvent>(string topic, Event message) where TEvent : Event;
    }
}
