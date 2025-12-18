using Features.Levels.Serialization;

namespace Common.Infrastructure
{
    public sealed class GlobalServices : IInitializable
    {
        public ISerializer Serializer { get; private set; }
        public IGamePersistenceService PersistenceService { get; private set; }

        public void Initialize()
        {
            Serializer = new Serializer();
            PersistenceService = new GamePersistenceService();
        }

        public void CleanUp() { }
    }
}