using Common.Infrastructure.Observation;

namespace Features.Combat.Logic
{
    public sealed class Combat
    {
        public Combatant Player { get; }
        public Combatant Bandits { get; }

        public Observable<int> RoundCounter { get; } = new();
        public ObservableEvent<CombatStatus> CombatResolved { get; } = new();

        public bool IsOver { get; private set; }

        public Combat(Combatant player, Combatant bandits)
        {
            Player = player;
            Bandits = bandits;
        }

        public float GuardHealthShare
        {
            get
            {
                var total = Player.TotalHealth.Value + Bandits.TotalHealth.Value;
                return total <= 0f ? 0.5f : Player.TotalHealth.Value / total;
            }
        }
    }
}