using System.Collections.Generic;
using Common.Infrastructure.Serialization;
using Features.Audio.Data;
using Features.Levels.Serialization;

namespace Common.Infrastructure.Global
{
    public sealed class PersistenceServices : IInitializable
    {
        public ISerializer Serializer { get; } = new Serializer();

        public AudioSettingsPersistenceService AudioSettingsPersistenceService { get; private set; }
        public GamePersistenceService GamePersistenceService { get; private set; }

        private readonly List<IService> _services = new();

        public void Initialize()
        {
            AudioSettingsPersistenceService = new AudioSettingsPersistenceService();
            GamePersistenceService = new GamePersistenceService();

            _services.Add(AudioSettingsPersistenceService);
            _services.Add(GamePersistenceService);

            foreach (var service in _services)
            {
                service.Initialize();
            }
        }

        public void CleanUp()
        {
            foreach (var service in _services)
            {
                service.CleanUp();
            }
        }
    }
}