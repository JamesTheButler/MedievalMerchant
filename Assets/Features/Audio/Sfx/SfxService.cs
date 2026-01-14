using Common.Infrastructure;
using Common.Infrastructure.Observation;

namespace Features.Audio.Sfx
{
    public sealed class SfxService : IService
    {
        public ObservableEvent<GameSoundEffect> GameSoundEffect { get; } = new();
        public ObservableEvent<UISoundEffect> UISoundEffect { get; } = new();

        public void Initialize() { }
        public void CleanUp() { }
    }
}