using System.Threading.Tasks;
using VsSummit2018.Domain;

namespace VsSummit2018.Infra.MessageBroker
{
    public class MessageSubscriberFactory : IMessageSubscriberFactory
    {
        private readonly IExchangeNameResolver exchangeNameResolver;
        private readonly IExchangeSubscriberService exchangeSubscriberService;

        public MessageSubscriberFactory(
            IExchangeNameResolver exchangeNameResolver,
            IExchangeSubscriberService exchangeSubscriberService)
        {
            this.exchangeNameResolver = exchangeNameResolver;
            this.exchangeSubscriberService = exchangeSubscriberService;
        }

        public async Task<MessageSubscriber<T>> CreateSubscriberAsync<T>(IMessageHandler<T> messageHandler)
            where T : Message
        {
            var exchangeName = exchangeNameResolver.GetExchangeName<T>();
            var subscriber = new MessageSubscriber<T>(exchangeSubscriberService)
            {
                MessageSubscriberInfo = new MessageSubscriberInfo
                {
                    ExchangeName = exchangeName,
                    QueueName = FormatQueueName(exchangeName)
                },
                MessageHandler = messageHandler
            };

            return await Task.FromResult(subscriber);
        }

        private static string FormatQueueName(string exchangeName)
        {
            return string.Format("{0}.Queue", exchangeName);
        }
    }
}
