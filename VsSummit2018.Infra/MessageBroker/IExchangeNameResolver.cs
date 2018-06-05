namespace VsSummit2018.Infra.MessageBroker
{
    public interface IExchangeNameResolver
    {
        string GetExchangeName(object message);

        string GetExchangeName<TMessage>()
            where TMessage : class;
    }
}
