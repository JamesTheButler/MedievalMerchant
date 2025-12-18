namespace Common.Infrastructure
{
    public interface IInitializable
    {
        void Initialize();
        void CleanUp();
    }
}