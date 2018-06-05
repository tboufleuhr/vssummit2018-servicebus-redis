namespace VsSummit2018.Infra
{
    public class AppConfiguration
    {
        public string Url { get; set; }
        public DataAccessConfiguration DataAccess { get; set; }
        public CachingConfiguration Caching { get; set; }
        public MessageBrokerConfiguration MessageBroker { get; set; }
        public RedisConfiguration Redis { get; set; }
    }
}
