using VsSummit2018.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VsSummit2018.Infra
{
    public sealed class InProcessBus : IBus
    {
        private readonly IMediator mediator;

        public InProcessBus(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public Task<TResponse> Send<TResponse>(CommandRequest<TResponse> command)
            where TResponse : CommandResponse
        {
            return mediator.Send<TResponse>(command);
        }

        public Task Send(Command command)
        {
            return mediator.Send(command);
        }

        public Task Publish(Message message)
        {
            return mediator.Publish(message);
        }
    }
}
