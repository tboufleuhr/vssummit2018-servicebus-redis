using VsSummit2018.Domain;
using Newtonsoft.Json;
using System.Text;

namespace VsSummit2018.Infra
{
    public class JsonSerializer : ISerializer, IMessageSerializer
    {
        public JsonSerializer()
        {
        }
        
        public T Deserialize<T>(byte[] content)
        {
            Ensure.Argument.NotNull(content, nameof(content));

            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(content));
        }

        public byte[] Serialize(object content)
        {
            Ensure.Argument.NotNull(content, nameof(content));

            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(content));
        }
    }
}
