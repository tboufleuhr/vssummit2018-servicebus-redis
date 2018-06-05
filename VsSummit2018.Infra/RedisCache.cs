using VsSummit2018.Domain;
using StackExchange.Redis;
using System;

namespace VsSummit2018.Infra
{
    public class RedisCache : ICache, IDisposable
    {
        private readonly IConnectionMultiplexer redis;
        private readonly IDatabase database;
        private readonly ISerializer serializer;
        private readonly CachingConfiguration configuration;

        public RedisCache(
            IConnectionMultiplexer redis, 
            ISerializer serializer, 
            CachingConfiguration configuration = null)
        {
            this.redis = redis;
            database = this.redis.GetDatabase();

            this.serializer = serializer;
            this.configuration = configuration;
        }

        public void Dispose()
        {
            if (redis != null)
            {
                redis.Dispose();
            }
        }

        public T Get<T>(string key)
        {
            Ensure.Argument.NotNullOrEmpty(key, nameof(key));

            var item = database.StringGet(key);
            if (item.IsNullOrEmpty)
            {
                return default(T);
            }

            return serializer.Deserialize<T>(item);
        }

        public void Remove(string key)
        {
            database.KeyDelete(key);
        }

        public void Set(string key, object item)
        {
            Ensure.Argument.NotNullOrEmpty(key, nameof(key));
            Ensure.Argument.NotNull(item, nameof(item));

            var serialized = serializer.Serialize(item);
            database.StringSet(key, serialized);
        }

        public void Set(string key, object item, TimeSpan expires)
        {
            Set(key, item);

            database.KeyExpire(key, expires, CommandFlags.FireAndForget);
        }
    }
}
