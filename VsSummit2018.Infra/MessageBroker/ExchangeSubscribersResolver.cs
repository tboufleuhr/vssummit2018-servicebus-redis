namespace VsSummit2018.Infra.MessageBroker
{
    public class ExchangeSubscribersResolver : IExchangeSubscribersResolver
    {
        public string GetSubscriberKey(string exchangeName)
        {
            return string.Format("{0}.Subscribers", exchangeName);
        }

        public string GetSubscriberKey<T>() where T : class
        {
            var exchangeName = GetExchangeName<T>();
            return GetSubscriberKey(exchangeName);
        }

        public string GetExchangeName(object message)
        {
            var fullName = message.GetType().FullName;
            return GenerateExchangeName(fullName);
        }

        public string GetExchangeName<T>() where T : class
        {
            var fullName = typeof(T).FullName;
            return GenerateExchangeName(fullName);
        }

        private static string GenerateExchangeName(string fullName)
        {
            return string.Format("Exchange.{0}", fullName);
        }
    }
}
