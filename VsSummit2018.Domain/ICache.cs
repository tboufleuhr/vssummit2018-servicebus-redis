using System;

namespace VsSummit2018.Domain
{
    public interface ICache
    {
        T Get<T>(string key);

        void Set(string key, object item);

        void Set(string key, object item, TimeSpan expires);

        void Remove(string key);
    }
}
