using System.Collections.Generic;
using System.Linq;
using Common.Config.Sampling;
using Common.Infrastructure.Modifiable;
using Common.Infrastructure.Observation;
using UnityEngine;

namespace Features.Combat.Logic
{
    public sealed class Combatant
    {
        public string Name { get; }
        public int Level { get; }

        public ModifiableVariable UnitHealth { get; }
        public ModifiableVariable UnitCombatStrength { get; }

        public IReadOnlyList<CombatUnit> Units => _units;
        public int UnitCount => _units.Count;

        public Observable<int> AliveCount { get; }
        public Observable<float> TotalHealth { get; }
        public Observable<float> TotalCombatStrength { get; }

        public Sprite CommanderIcon { get; }
        public Sprite UnitIcon { get; }
        public IEnumerable<CombatUnit> AliveUnits => _units.Where(unit => unit.IsAlive.Value);

        public bool IsAlive => AliveCount.Value > 0;
        public ISampler HitSampler { get; }
        public string UnitName { get; set; }

        private readonly List<CombatUnit> _units;

        public Combatant(
            string name,
            int level,
            int unitCount,
            float unitHealth,
            float unitCombatStrength,
            string healthDescription,
            string combatStrengthDescription,
            ISampler hitSampler,
            Sprite commanderIcon,
            Sprite unitIcon,
            string unitName)
        {
            Level = level;
            HitSampler = hitSampler;
            CommanderIcon = commanderIcon;
            UnitIcon = unitIcon;
            UnitName = unitName;
            Name = name;

            var baseUnitHealth = new CombatBaseValue(unitHealth, healthDescription);
            var baseUnitStrength = new CombatBaseValue(unitCombatStrength, combatStrengthDescription);

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