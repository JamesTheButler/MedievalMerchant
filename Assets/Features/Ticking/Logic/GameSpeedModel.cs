using Common.Infrastructure.Observation;

namespace Features.Ticking.Logic
{
    public sealed class GameSpeedModel
    {
        public IReadOnlyObservable<bool> IsPaused => _isPaused;
        public Observable<GameSpeed> GameSpeed { get; } = new();

        private readonly Observable<bool> _isPaused = new();

        public void Pause()
        {
            _isPaused.Value = true;
        }

        public void Resume()
        {
            _isPaused.Value = false;
        }
    }
}