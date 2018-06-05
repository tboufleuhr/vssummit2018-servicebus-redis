using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VsSummit2018.Domain;
using StackExchange.Redis;

namespace VsSummit2018.Infra.MessageBroker
{
    public class RedisExchangeSubscriberService : IExchangeSubscriberService
    {
        private readonly IConnectionMultiplexer connectionMultiplexer;
        private readonly IDatabase database;
        private readonly IExchangeSubscribersResolver exchangeSubscribersResolver;
        private readonly IMessageSerializer messageSerializer;

        public RedisExchangeSubscriberService(
            IConnectionMultiplexer connectionMultiplexer,
            IExchangeSubscribersResolver exchangeSubscribersResolver,
            IMessageSerializer messageSerializer)
        {
            this.connectionMultiplexer = connectionMultiplexer;
            database = connectionMultiplexer.GetDatabase();
            this.exchangeSubscribersResolver = exchangeSubscribersResolver;
            this.messageSerializer = messageSerializer;
        }

        public async Task AddSubscriberAsync<TMessage>(MessageSubscriber<TMessage> messageSubscriber)
            where TMessage : Message
        {
            var subscriberKey = exchangeSubscribersResolver.GetSubscriberKey<TMessage>();
            var serialized = messageSerializer.Serialize(messageSubscriber.MessageSubscriberInfo);

            await database.SetAddAsync(subscriberKey, serialized);
        }

        public async Task<List<MessageSubscriberInfo>> GetSubscriberInfosAsync<TMessage>()
            where TMessage : class
        {
            var subscriberKey = exchangeSubscribersResolver.GetSubscriberKey<TMessage>();
            var subscribers = await database.SetMembersAsync(subscriberKey);

            return subscribers
                .Select(m => m.IsNull ? null : messageSerializer.Deserialize<MessageSubscriberInfo>(m))
                .ToList();
        }

        public async Task PushMessageToSubscriberAsync<TMessage>(MessageSubscriberInfo messageSubscriberInfo, TMessage message)
            where TMessage : class
        {
            var serialized = messageSerializer.Serialize(message);

            await database.ListRightPushAsync(messageSubscriberInfo.QueueName, serialized);
        }

        public async Task SubscribeAsync<TMessage>(MessageSubscriberInfo messageSubscriberInfo, Func<TMessage, Task> onMessageAsync)
        {
            var subscriber = connectionMultiplexer.GetSubscriber();

            await subscriber.SubscribeAsync(
                messageSubscriberInfo.ExchangeName,
                async (redisChannel, value) => await onMessageAsync(messageSerializer.Deserialize<TMessage>(value)));
        }

        public async Task NotifyOfNewMessagesAsync(object message)
        {
            var exchangeName = exchangeSubscribersResolver.GetExchangeName(message);

            var sub = connectionMultiplexer.GetSubscriber();
            await sub.PublishAsync(exchangeName, await Task.Run(() => messageSerializer.Serialize(message)));
        }

        public async Task<TMessage> GetNextMessageAsync<TMessage>(MessageSubscriberInfo messageSubscriberInfo)
        {
            var item = await database.ListLeftPopAsync(messageSubscriberInfo.QueueName);

            return item.IsNull ? default(TMessage) : await Task.Run(() => messageSerializer.Deserialize<TMessage>(item));
        }
    }
}
