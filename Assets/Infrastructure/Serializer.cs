using Newtonsoft.Json;

namespace Infrastructure
{
    public class Serializer : ISerializer
    {
        public T Deserialize<T>(string input)
        {
            return JsonConvert.DeserializeObject<T>(input);
        }

        public string Serialize<T>(T input)
        {
            return JsonConvert.SerializeObject(input);
        }
    }
}