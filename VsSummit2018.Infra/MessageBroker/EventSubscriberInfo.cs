using System.Threading;

namespace VsSummit2018.Infra.MessageBroker
{
    public class EventSubscriberInfo
    {
        public CancellationToken CancellationToken { get; set; }
        public string Topic { get; set; }
    }
}
