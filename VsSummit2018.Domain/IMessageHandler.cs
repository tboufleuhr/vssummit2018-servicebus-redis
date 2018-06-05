using System.Threading.Tasks;

namespace VsSummit2018.Domain
{
    public interface IMessageHandler<TMessage> 
        where TMessage : Message
    {
        Task Handle(TMessage message);
    }
}
