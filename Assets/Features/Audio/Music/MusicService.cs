using Common.Infrastructure;
using Common.Infrastructure.Observation;

namespace Features.Audio.Music
{
    public sealed class MusicService : IService
    {
        public ObservableEvent<MusicMode> MusicModeChange { get; } = new();

        public void Initialize() { }
        public void CleanUp() { }
    }
}