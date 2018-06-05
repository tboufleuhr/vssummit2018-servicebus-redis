using System.Threading.Tasks;

namespace VsSummit2018.Domain
{
    public interface IMessageBroker
    {
        Task PublishAsync<TEvent>(string topic, TEvent message)
             where TEvent : Event;

        Task ReceiveAsync<TMessage>(IMessageHandler<TMessage> messageHandler = null)
            where TMessage : Message;

        Task SendAsync<TMessage>(TMessage message)
            where TMessage : Message;

        Task SubscribeAsync<TEvent>(string topic, IEventHandler<TEvent> eventHandler = null)
            where TEvent : Event;
    }
}
