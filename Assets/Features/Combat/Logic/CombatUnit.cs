using System.Collections.Generic;
using Common.Infrastructure.Modifiable;
using Common.Infrastructure.Observation;

namespace Features.Combat.Logic
{
    public sealed class CombatUnit
    {
        public Combatant Combatant { get; }
        public float MaxHealth { get; }
        public Observable<float> Health { get; }
        public Observable<float> DamageTaken { get; } = new();
        public Observable<bool> IsAlive { get; }
        public IReadOnlyList<IModifier> ActiveEffects => Combatant.UnitCombatStrength.Modifiers;

        public CombatUnit(Combatant combatant, float maxHealth)
        {
            Combatant = combatant;
            MaxHealth = maxHealth;

            Health = new Observable<float>(maxHealth);
            IsAlive = new Observable<bool>(true);
        }

        public void ReceiveDamage(float damage)
        {
            if (!IsAlive.Value || damage <= 0f)
                return;

            DamageTaken.Value += damage;
            Health.Value -= damage;

            if (Health.Value <= 0f)
            {
                Die();
            }
        }

        private void Die()
        {
            Health.Value = 0f;
            IsAlive.Value = false;
        }
    }
}