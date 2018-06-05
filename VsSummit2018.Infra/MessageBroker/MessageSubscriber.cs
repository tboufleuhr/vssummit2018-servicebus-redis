using System;
using System.Threading.Tasks;
using VsSummit2018.Domain;

namespace VsSummit2018.Infra.MessageBroker
{
    public class MessageSubscriber<TMessage>
        where TMessage : Message
    {
        private readonly IExchangeSubscriberService exchangeSubscriberService;

        public IMessageHandler<TMessage> MessageHandler { get; set; }

        public MessageSubscriberInfo MessageSubscriberInfo { get; set; }

        public MessageSubscriber(IExchangeSubscriberService exchangeSubscriberService)
        {
            Ensure.Argument.NotNull(exchangeSubscriberService, nameof(exchangeSubscriberService));

            this.exchangeSubscriberService = exchangeSubscriberService;
        }

        private async Task DoWorkAsync(TMessage arg)
        {
            if (MessageHandler == null)
            {
                return;
            }

            TMessage message;
            while ((message = await exchangeSubscriberService.GetNextMessageAsync<TMessage>(MessageSubscriberInfo)) != null)
            {
                try
                {
                    await MessageHandler.Handle(message);
                }
                catch
                {
                    await PushAsync(message);
                    throw;
                }
            }
        }

        private async Task PushAsync(TMessage message)
        {
            await exchangeSubscriberService.PushMessageToSubscriberAsync(MessageSubscriberInfo, message);
        }

        public void Subscribe()
        {
            exchangeSubscriberService.SubscribeAsync<TMessage>(MessageSubscriberInfo, DoWorkAsync);

            DoWorkAsync(null).FireAndForget();
        }
    }
}
