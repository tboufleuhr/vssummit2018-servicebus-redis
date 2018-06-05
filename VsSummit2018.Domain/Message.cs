using MediatR;
using System;

namespace VsSummit2018.Domain
{
    public abstract class Message : INotification, IRequest
    {
        public readonly Guid Identifier = Guid.NewGuid();
        public readonly DateTime Timestamp = DateTime.UtcNow;
    }
}
