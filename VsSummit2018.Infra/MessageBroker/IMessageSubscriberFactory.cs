using System.Threading.Tasks;
using VsSummit2018.Domain;

namespace VsSummit2018.Infra.MessageBroker
{
    public interface IMessageSubscriberFactory
    {
        Task<MessageSubscriber<TMessage>> CreateSubscriberAsync<TMessage>(IMessageHandler<TMessage> messageHandler)
            where TMessage : Message;
    }
}
