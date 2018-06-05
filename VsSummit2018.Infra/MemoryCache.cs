using System;
using VsSummit2018.Domain;
using Microsoft.Extensions.Caching.Memory;

namespace VsSummit2018.Infra
{
    public class MemoryCache : ICache
    {
        private readonly IMemoryCache memory;

        public MemoryCache(IMemoryCache memory)
        {
            this.memory = memory;
        }

        public T Get<T>(string key)
        {
            return memory.Get<T>(key);
        }

        public void Remove(string key)
        {
            memory.Remove(key);
        }

        public void Set(string key, object item)
        {
            memory.Set(key, item);
        }

        public void Set(string key, object item, TimeSpan expires)
        {
            memory.Set(key, item, expires);
        }
    }
}
