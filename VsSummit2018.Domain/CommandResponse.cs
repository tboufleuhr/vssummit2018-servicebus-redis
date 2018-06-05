using System;
using System.Collections.Generic;
using System.Text;

namespace VsSummit2018.Domain
{
    public class CommandResponse : Message
    {
        public virtual string Message { get; protected set; }
        public virtual Exception Exception { get; set; }
        public virtual bool Success => Exception != null;

        public CommandResponse()
        {

        }
    }

    public class CommandResponse<TPayload> : CommandResponse
    {
        public TPayload Payload { get; set; }

        public CommandResponse()
        {

        }
    }
}
