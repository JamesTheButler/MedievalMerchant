using Common.Infrastructure.Observation;

namespace Features.Combat
{
    public sealed class CombatUnit
    {
        public Observable<float> Health { get; }
        public Observable<bool> IsAlive { get; }
        public Observable<float> Strength { get; }

        public CombatUnit(float health, float strength)
        {
            Health = new Observable<float>(health);
            IsAlive = new Observable<bool>(true);
            Strength = new Observable<float>(strength);
        }

        public void ReceiveDamage(float damage)
        {
            Health.Value -= damage;
            if (Health.Value <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Health.Value = 0;
            IsAlive.Value = false;
        }
    }
}