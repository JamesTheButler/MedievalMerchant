using Common.Infrastructure.Observation;

namespace Features.Combat.Logic
{
    public sealed class Combat
    {
        public Combatant Player { get; }
        public Combatant Bandits { get; }
        public Observable<int> RoundCounter { get; } = new();
        public ObservableEvent<CombatStatus> CombatResolved { get; } = new();
        public Observable<float> GuardHealthShare { get; }

        public bool IsOver { get; private set; }

        public Combat(Combatant player, Combatant bandits)
        {
            Player = player;
            Bandits = bandits;

            GuardHealthShare = ObservableExtensions.Combine(
                Player.TotalHealth,
                Bandits.TotalHealth,
                RefreshGuardHealthShare);
        }

        private static float RefreshGuardHealthShare(float playerHealth, float banditHealth)
        {
            var total = playerHealth + banditHealth;
            return total <= 0f ? 0.5f : playerHealth / total;
        }
    }
}