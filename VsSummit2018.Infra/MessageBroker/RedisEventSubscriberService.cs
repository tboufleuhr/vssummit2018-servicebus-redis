using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VsSummit2018.Domain;
using StackExchange.Redis;

namespace VsSummit2018.Infra.MessageBroker
{
    public class RedisEventSubscriberService : IEventSubscriberService
    {
        private readonly IMessageSerializer messageSerializer;
        private readonly IDictionary<string, ISubscriber> topicSubscribers;

        public RedisEventSubscriberService(IMessageSerializer messageSerializer)
        {
            this.messageSerializer = messageSerializer;
            topicSubscribers = new Dictionary<string, ISubscriber>();
        }

        public async Task AddSubscriberAsync<TEvent>(EventSubscriber<TEvent> eventSubscriber) where TEvent : Event
        {
            var subscribe = eventSubscriber.Subscribe();

            if (!topicSubscribers.ContainsKey(eventSubscriber.EventSubscriberInfo.Topic))
            {
                topicSubscribers.Add(eventSubscriber.EventSubscriberInfo.Topic, subscribe);
            }

            await subscribe.SubscribeAsync(eventSubscriber.EventSubscriberInfo.Topic,
                (channel, value) => HandlEvent(value, eventSubscriber.EventHandler, eventSubscriber.EventSubscriberInfo.CancellationToken));
        }

        public Task PublishAsync<TEvent>(string topic, Event message) where TEvent : Event
        {
            if (topicSubscribers.ContainsKey(topic))
            {
                var serialized = messageSerializer.Serialize(message);

                return Task.Run(() => { topicSubscribers[topic].PublishAsync(topic, serialized); });
            }

            return Task.CompletedTask;
        }

        private void HandlEvent<TEvent>(RedisValue value, IEventHandler<TEvent> eventHandler, CancellationToken cancellationToken) where TEvent : Event
        {
            var deserialized = messageSerializer.Deserialize<TEvent>(value);

            eventHandler.Handle(deserialized, cancellationToken);
        }
    }
}
