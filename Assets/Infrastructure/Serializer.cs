using Common;

namespace Infrastructure
{
    public class Serializer : ISerializer
    {
        public T Deserialize<T>(string input)
        {
            return SafeJsonUtility.FromJson<T>(input);
        }
        
        public string Serialize<T>(T input)
        {
            return "";
        }
    }
}