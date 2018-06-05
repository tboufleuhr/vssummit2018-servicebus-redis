using MediatR;
using System;

namespace VsSummit2018.Domain
{
    public abstract class Command : Message
    {
        protected Command()
        {
        }
    }
}
