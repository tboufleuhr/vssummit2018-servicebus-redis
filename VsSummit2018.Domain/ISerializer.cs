using System;
using System.Collections.Generic;
using System.Text;

namespace VsSummit2018.Domain
{
    public interface ISerializer
    {
        byte[] Serialize(object content);

        T Deserialize<T>(byte[] content);
    }
}
