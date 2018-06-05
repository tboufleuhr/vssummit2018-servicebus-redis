using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace VsSummit2018.Domain
{
    public interface ICommandRequestHandler<in TCommand, TCommandResponse> : IRequestHandler<TCommand, TCommandResponse> 
        where TCommand : CommandRequest<TCommandResponse>
        where TCommandResponse : CommandResponse
    {
    }

    public interface ICommandRequestHandler<in TCommand> : IRequestHandler<TCommand, CommandResponse>
        where TCommand : CommandRequest<CommandResponse>
    {
    }
}
