using System.Collections.Generic;
using System.Linq;
using Common.Infrastructure.Observation;
using Common.Utility;

namespace Features.Combat
{
    public sealed class Combat
    {
        public Observable<CombatStatus> Status { get; } = new();

        private readonly Combatant _attacker, _defender;

        public Combat(Combatant attacker, Combatant defender)
        {
            _attacker = attacker;
            _defender = defender;
        }

        public PreparedAttacks PrepareRound()
        {
            var attackerAttacks = PrepareAttacks(_attacker, _defender);
            var defenderAttacks = PrepareAttacks(_defender, _attacker);
            return new PreparedAttacks(attackerAttacks, defenderAttacks);
        }

        public void ExecuteRound(PreparedAttacks preparedAttacks)
        {
            preparedAttacks.AttackerAttacks.ForEach(ResolveAttack);
            preparedAttacks.DefenderAttacks.ForEach(ResolveAttack);

            var attackerAlive = _attacker.AliveUnits.Count > 0;
            var defenderAlive = _defender.AliveUnits.Count > 0;

            var status = (attackerAlive, defenderAlive) switch
            {
                (true, true) => CombatStatus.Ongoing,
                (false, false) => CombatStatus.Draw,
                (true, false) => CombatStatus.AttackerWins,
                (false, true) => CombatStatus.DefenderWins
            };

            Status.Value = status;
        }

        private IReadOnlyCollection<Attack> PrepareAttacks(Combatant attacker, Combatant defender)
        {
            if (defender.AliveUnits.Count == 0)
                return new List<Attack>();

            var aliveAttackingUnits = attacker.Units
                .Where(unit => unit.IsAlive.Value)
                .ToArray();

            var attacks = new List<Attack>(aliveAttackingUnits.Length);
            foreach (var unit in aliveAttackingUnits)
            {
                // TODO: we should select attacked units based on closeness to the attacking unit
                var attack = new Attack(unit, defender.AliveUnits.GetRandom(), attacker.HitFactor());
                attacks.Add(attack);
            }

            return attacks;
        }

        private static void ResolveAttack(Attack attack)
        {
            attack.Defender.ReceiveDamage(attack.Attacker.Strength * attack.HitFactor);
        }
    }
}