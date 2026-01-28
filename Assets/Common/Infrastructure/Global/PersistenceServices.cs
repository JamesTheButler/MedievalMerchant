using System.Collections.Generic;
using Common.Infrastructure.Serialization;
using Features.Audio.Data;
using Features.Levels.Serialization;
using Features.Tutorial.Logic;

namespace Common.Infrastructure.Global
{
    public sealed class PersistenceServices : IInitializable
    {
        public ISerializer Serializer { get; } = new Serializer();

        public TutorialPersistenceService TutorialPersistenceService { get; private set; }
        public AudioSettingsPersistenceService AudioSettingsPersistenceService { get; private set; }
        public GamePersistenceService GamePersistenceService { get; private set; }

        private readonly List<IService> _services = new();

        public void Initialize()
        {
            TutorialPersistenceService = new TutorialPersistenceService();
            AudioSettingsPersistenceService = new AudioSettingsPersistenceService();
            GamePersistenceService = new GamePersistenceService();

            _services.Add(TutorialPersistenceService);
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