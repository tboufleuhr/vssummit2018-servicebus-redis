using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VsSummit2018.Domain;

namespace VsSummit2018.Infra.MessageBroker
{
    public interface IExchangeSubscriberService
    {
        Task AddSubscriberAsync<TMessage>(MessageSubscriber<TMessage> messageSubscriber)
            where TMessage : Message;

        Task<TMessage> GetNextMessageAsync<TMessage>(MessageSubscriberInfo messageSubscriberInfo);

        Task<List<MessageSubscriberInfo>> GetSubscriberInfosAsync<TMessage>()
            where TMessage : class;

        Task NotifyOfNewMessagesAsync(object message);

        Task PushMessageToSubscriberAsync<TMessage>(MessageSubscriberInfo messageSubscriberInfo, TMessage message)
            where TMessage : class;

        Task SubscribeAsync<TMessage>(MessageSubscriberInfo messageSubscriberInfo, Func<TMessage, Task> onMessageAsync);
    }
}
