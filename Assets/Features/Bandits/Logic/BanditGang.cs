using Common.Infrastructure.Observation;

namespace Features.Bandits.Logic
{
    public sealed class BanditGang
    {
        public Observable<BanditTier> Tier { get; } = new();
        public Observable<int> UnitCount { get; } = new();
        public Observable<float> UnitHealth { get; } = new();
        public Observable<float> UnitCombatStrength { get; } = new();
    }
}