using System.Collections.Concurrent;
using System.Collections.Generic;
using VsSummit2018.Domain;
using StackExchange.Redis;

namespace VsSummit2018.Infra.MessageBroker
{
    public class EventSubscriber<TEvent>
        where TEvent : Event
    {
        private readonly IConnectionMultiplexer connectionMultiplexer;
        private readonly IDictionary<string, ISubscriber> subscribers;

        public IEventHandler<TEvent> EventHandler { get; set; }

        public EventSubscriberInfo EventSubscriberInfo { get; set; }

        public EventSubscriber(IConnectionMultiplexer connectionMultiplexer)
        {
            Ensure.Argument.NotNull(connectionMultiplexer, nameof(connectionMultiplexer));

            this.connectionMultiplexer = connectionMultiplexer;
            subscribers = new ConcurrentDictionary<string, ISubscriber>();
        }

        public ISubscriber Subscribe()
        {
            if (!subscribers.ContainsKey(EventSubscriberInfo.Topic))
            {
                subscribers.Add(EventSubscriberInfo.Topic, connectionMultiplexer.GetSubscriber(EventSubscriberInfo.Topic));
            }

            return subscribers[EventSubscriberInfo.Topic];
        }
    }
}
