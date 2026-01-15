using Common.Infrastructure.Observation;

namespace Features.Settings.Logic
{
    public sealed class AudioSettingsModel
    {
        public Observable<int> TotalVolume { get; } = new();
        public Observable<int> MusicVolume { get; } = new();
        public Observable<int> SfxVolume { get; } = new();
        public Observable<int> InterfaceVolume { get; } = new();

        public void Initialize()
        {
            TotalVolume.Value = 50;
            MusicVolume.Value = 50;
            SfxVolume.Value = 50;
            InterfaceVolume.Value = 50;
        }
    }
}