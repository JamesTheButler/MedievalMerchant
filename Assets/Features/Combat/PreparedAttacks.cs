using System.Collections.Generic;

namespace Features.Combat
{
    public sealed class PreparedAttacks
    {
        public IReadOnlyCollection<Attack> AttackerAttacks { get; }
        public IReadOnlyCollection<Attack> DefenderAttacks { get; }

        public PreparedAttacks(
            IReadOnlyCollection<Attack> attackerAttacks,
            IReadOnlyCollection<Attack> defenderAttacks)
        {
            AttackerAttacks = attackerAttacks;
            DefenderAttacks = defenderAttacks;
        }
    }
}