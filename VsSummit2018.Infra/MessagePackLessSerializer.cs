using VsSummit2018.Domain;
using MessagePack;

namespace VsSummit2018.Infra
{
    public class MessagePackLessSerializer : ISerializer, IMessageSerializer
    {
        public MessagePackLessSerializer()
        {
            MessagePackSerializer.SetDefaultResolver(MessagePack.Resolvers.ContractlessStandardResolver.Instance);
        }
        
        public T Deserialize<T>(byte[] content)
        {
            Ensure.Argument.NotNull(content, nameof(content));

            return MessagePackSerializer.Deserialize<T>(content);
        }

        public byte[] Serialize(object content)
        {
            Ensure.Argument.NotNull(content, nameof(content));

            return MessagePackSerializer.Serialize(content);
        }
    }
}
