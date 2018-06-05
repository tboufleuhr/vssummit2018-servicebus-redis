using VsSummit2018.Domain;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace VsSummit2018.Infra.MessageBroker
{
    public class MessageBroker : IMessageBroker
    {
        private readonly ICache cache;
        private readonly IEventSubscriberFactory eventSubscriberFactory;
        private readonly IEventSubscriberService eventSubscriberService;
        private readonly SemaphoreSlim eventSubscribeSemaphore = new SemaphoreSlim(1);

        private readonly IExchangeSubscriberService exchangeSubscriberService;
        private readonly IMessageSubscriberFactory messageSubscriberFactory;
        private readonly SemaphoreSlim messageSubscribeSemaphore = new SemaphoreSlim(1);
        private readonly IServiceProvider serviceProvider;

        public MessageBroker(
            IExchangeSubscriberService exchangeSubscriberService,
            IMessageSubscriberFactory messageSubscriberFactory,
            IEventSubscriberFactory eventSubscriberFactory,
            IEventSubscriberService eventSubscriberService,
            ICache cache,
            IServiceProvider serviceProvider)
        {
            this.exchangeSubscriberService = exchangeSubscriberService;
            this.messageSubscriberFactory = messageSubscriberFactory;
            this.eventSubscriberFactory = eventSubscriberFactory;
            this.eventSubscriberService = eventSubscriberService;
            this.cache = cache;
            this.serviceProvider = serviceProvider;
        }

        public async Task SendAsync<TMessage>(TMessage message)
            where TMessage : Message
        {
            List<MessageSubscriberInfo> subscriberInfos;

            try
            {
                await messageSubscribeSemaphore.WaitAsync();

                subscriberInfos = await GetSubscriberInfosAsync<TMessage>();
                if (!subscriberInfos.Any())
                {
                    var createSubscriber = await CreateSubscriberAsync<TMessage>(null);
                    subscriberInfos.Add(createSubscriber.MessageSubscriberInfo);
                }
            }
            finally
            {
                messageSubscribeSemaphore.Release();
            }

            var tasks = new List<Task>();
            foreach (var subscriberInfo in subscriberInfos)
            {
                tasks.Add(exchangeSubscriberService.PushMessageToSubscriberAsync(subscriberInfo, message));
            }

            await Task.WhenAll(tasks);

            await exchangeSubscriberService.NotifyOfNewMessagesAsync(message);
        }

        public async Task ReceiveAsync<TMessage>(IMessageHandler<TMessage> messageHandler = null)
            where TMessage : Message
        {
            var handler = messageHandler ?? serviceProvider.GetService<IMessageHandler<TMessage>>();
            if (handler == null)
            {
                throw new InvalidOperationException($"There is no handler registration for message type '{typeof(TMessage)}'");
            }

            await CreateSubscriberAsync(handler);
        }

        public async Task SubscribeAsync<TEvent>(string topic, IEventHandler<TEvent> eventHandler = null) where TEvent : Event
        {
            await eventSubscribeSemaphore.WaitAsync();

            var eventHandlerInternal = eventHandler ?? serviceProvider.GetService<IEventHandler<TEvent>>();
            if (eventHandlerInternal == null)
            {
                throw new InvalidOperationException($"There is no event handler registration for event type '{typeof(TEvent)}'");
            }

            var subscriber = await eventSubscriberFactory.CreateSubscriberAsync(topic, eventHandlerInternal);

            await eventSubscriberService.AddSubscriberAsync(subscriber);

            eventSubscribeSemaphore.Release();
        }

        public Task PublishAsync<TEvent>(string topic, TEvent message) where TEvent : Event
        {
            return eventSubscriberService.PublishAsync<TEvent>(topic, message);
        }

        private async Task<MessageSubscriber<TMessage>> CreateSubscriberAsync<TMessage>(IMessageHandler<TMessage> messageHandler)
            where TMessage : Message
        {
            var subscriber = await messageSubscriberFactory.CreateSubscriberAsync(messageHandler);
            await exchangeSubscriberService.AddSubscriberAsync(subscriber);

            if (messageHandler != null)
            {
                subscriber.Subscribe();
            }

            return subscriber;
        }

        private async Task<List<MessageSubscriberInfo>> GetSubscriberInfosAsync<TMessage>()
            where TMessage : class
        {
            var typeName = typeof(TMessage).FullName;
            var cachedSubscriberInfos = cache.Get<List<MessageSubscriberInfo>>(typeName);
            if (cachedSubscriberInfos != null)
            {
                return cachedSubscriberInfos;
            }

            var subscriberInfos = await exchangeSubscriberService.GetSubscriberInfosAsync<TMessage>();
            cache.Set(typeName, subscriberInfos, TimeSpan.FromMinutes(1));

            return subscriberInfos;
        }
    }
}
