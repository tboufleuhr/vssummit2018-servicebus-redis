using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VsSummit2018.Domain
{
    public interface IBus
    {
        Task<TResponse> Send<TResponse>(CommandRequest<TResponse> command)
            where TResponse : CommandResponse;

        Task Send(Command command);

        Task Publish(Message message);
    }
}
