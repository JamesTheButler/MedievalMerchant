namespace Common.Infrastructure.Serialization
{
    public interface ISerializer
    {
        public T Deserialize<T>(string input);
        public string Serialize<T>(T input);
    }
}