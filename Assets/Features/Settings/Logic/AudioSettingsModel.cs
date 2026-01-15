using Common.Infrastructure.Global;
using Common.Infrastructure.Observation;

namespace Features.Settings.Logic
{
    public sealed class AudioSettingsModel
    {
        public Observable<int> MasterVolume { get; } = new();
        public Observable<int> MusicVolume { get; } = new();
        public Observable<int> SfxVolume { get; } = new();
        public Observable<int> InterfaceVolume { get; } = new();

        public void Initialize()
        {
            var persistenceService = GlobalContext.Instance.PersistenceServices.AudioSettingsPersistenceService;

            var storedSettings = persistenceService.Load();
            MasterVolume.Value = storedSettings.MasterVolume;
            MusicVolume.Value = storedSettings.MusicVolume;
            SfxVolume.Value = storedSettings.SfxVolume;
            InterfaceVolume.Value = storedSettings.InterfaceVolume;
        }
    }
}