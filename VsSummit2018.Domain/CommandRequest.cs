using MediatR;
using System;

namespace VsSummit2018.Domain
{
    public abstract class CommandRequest<TResponse> : Command, IRequest<TResponse>
        where TResponse : CommandResponse
    {
 
    }
}
