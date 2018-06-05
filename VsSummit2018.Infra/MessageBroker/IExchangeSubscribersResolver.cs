namespace VsSummit2018.Infra.MessageBroker
{
    public interface IExchangeSubscribersResolver : IExchangeNameResolver
    {
        string GetSubscriberKey(string exchangeName);

        string GetSubscriberKey<TMessage>()
            where TMessage : class;
    }
}
