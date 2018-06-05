using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace VsSummit2018.Domain
{
    public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand> 
        where TCommand : Command
    {
    }
}
