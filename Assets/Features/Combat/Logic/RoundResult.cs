using System.Collections.Generic;

namespace Features.Combat.Logic
{
    public sealed class RoundResult
    {
        public int Round { get; init; }
        public IReadOnlyList<Attack> Attacks { get; init; } = new List<Attack>();
        public IReadOnlyList<CombatUnit> Fallen { get; init; } = new List<CombatUnit>();
        public CombatantDelta Guards { get; init; } = CombatantDelta.None;
        public CombatantDelta Bandits { get; init; } = CombatantDelta.None;
        public CombatStatus Status { get; init; } = CombatStatus.Ongoing;
    }
}