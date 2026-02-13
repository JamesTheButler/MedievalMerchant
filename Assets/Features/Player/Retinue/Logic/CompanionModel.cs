using Common.Infrastructure.Observation;

namespace Features.Player.Retinue.Logic
{
    public sealed class CompanionModel
    {
        public CompanionType CompanionType { get; }
        public IReadOnlyObservable<int> Level => _level;

        private readonly Observable<int> _level = new();

        public CompanionModel(CompanionType companionType)
        {
            CompanionType = companionType;
        }

        public void SetLevel(int newLevel)
        {
            _level.Value = newLevel;
        }
    }
}