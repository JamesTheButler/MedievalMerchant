using System.Collections.Generic;
using System.Linq;
using Common.Config.Sampling;
using Common.Infrastructure.Modifiable;
using Common.Infrastructure.Observation;

namespace Features.Combat.Logic
{
    public sealed class Combatant
    {
        public int Level { get; }

        public ModifiableVariable UnitHealth { get; }
        public ModifiableVariable UnitCombatStrength { get; }

        public IReadOnlyList<CombatUnit> Units => _units;
        public int UnitCount => _units.Count;

        public Observable<int> AliveCount { get; }
        public Observable<float> TotalHealth { get; }
        public Observable<float> TotalCombatStrength { get; }

        public IEnumerable<CombatUnit> AliveUnits => _units.Where(unit => unit.IsAlive.Value);

        public bool IsAlive => AliveCount.Value > 0;
        public ISampler HitSampler { get; }

        private readonly List<CombatUnit> _units;

        public Combatant(
            int level,
            int unitCount,
            float baseHealth,
            float baseCombatStrength,
            string healthDescription,
            string combatStrengthDescription,
            ISampler hitSampler)
        {
            Level = level;
            HitSampler = hitSampler;

            var baseUnitHealth = new CombatBaseValue(baseHealth, healthDescription);
            var baseUnitStrength = new CombatBaseValue(baseCombatStrength, combatStrengthDescription);

            UnitHealth = new ModifiableVariable(healthDescription, true, baseUnitHealth);
            UnitCombatStrength = new ModifiableVariable(combatStrengthDescription, true, baseUnitStrength);

            _units = new List<CombatUnit>(unitCount);
            for (var i = 0; i < unitCount; i++)
            {
                _units.Add(new CombatUnit(this, UnitHealth.Value));
            }

            TotalHealth = new ObservableSum(_units.Select(unit => unit.Health));
            AliveCount = new ObservableFilter<bool>(_units.Select(unit => unit.IsAlive), isAlive => isAlive);

            TotalCombatStrength = ObservableExtensions.Combine(
                AliveCount,
                UnitCombatStrength,
                RefreshTotalCombatStrength);
        }

        private static float RefreshTotalCombatStrength(int aliveCount, float unitCombatStrength)
        {
            return aliveCount * unitCombatStrength;
        }
    }
}