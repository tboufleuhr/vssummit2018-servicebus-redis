using System.Threading;
using System.Threading.Tasks;
using VsSummit2018.Domain;
using StackExchange.Redis;

namespace VsSummit2018.Infra.MessageBroker
{
    public class EventSubscriberFactory : IEventSubscriberFactory
    {
        private readonly IConnectionMultiplexer connectionMultiplexer;

        public EventSubscriberFactory(IConnectionMultiplexer connectionMultiplexer)
        {
            this.connectionMultiplexer = connectionMultiplexer;
        }

        public async Task<EventSubscriber<TEvent>> CreateSubscriberAsync<TEvent>(string topic, IEventHandler<TEvent> eventHandler) where TEvent : Event
        {
            var subscriber = new EventSubscriber<TEvent>(connectionMultiplexer)
            {
                EventHandler = eventHandler,
                EventSubscriberInfo = new EventSubscriberInfo { Topic = topic, CancellationToken = new CancellationToken() }
            };

            return await Task.FromResult(subscriber);
        }
    }
}
