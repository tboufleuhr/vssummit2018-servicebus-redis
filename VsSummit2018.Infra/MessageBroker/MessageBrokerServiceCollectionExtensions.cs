using VsSummit2018.Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace VsSummit2018.Infra.MessageBroker
{
    public static class MessageBrokerServiceCollectionExtensions
    {
        public static void AddMessageBroker(this IServiceCollection services)
        {
            services.AddSingleton<IConnectionMultiplexer>(provider =>
            {
                var brokerConfig = provider.GetService<IOptions<RedisConfiguration>>();
                Ensure.NotNull(brokerConfig.Value, "There is no configuration for MessageBroker");

                var configuration = brokerConfig.Value;

                return ConnectionMultiplexer.Connect($"{configuration.Server},connectTimeout={configuration.ConnectTimeout},syncTimeout={configuration.SyncTimeout}");
            });

            services.AddSingleton<IMessageSerializer, MessagePackLessSerializer>();

            services.AddScoped<IExchangeNameResolver, ExchangeSubscribersResolver>();
            services.AddScoped<IExchangeSubscribersResolver, ExchangeSubscribersResolver>();
            services.AddScoped<IExchangeSubscriberService, RedisExchangeSubscriberService>();
            services.AddScoped<IEventSubscriberService, RedisEventSubscriberService>();
            services.AddScoped<IMessageSubscriberFactory, MessageSubscriberFactory>();
            services.AddScoped<IEventSubscriberFactory, EventSubscriberFactory>();
            services.AddScoped<IMessageBroker, MessageBroker>();
        }
    }
}
