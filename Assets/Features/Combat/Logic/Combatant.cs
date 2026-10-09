using System.Collections.Generic;
using System.Linq;
using Common.Config.Sampling;
using Common.Infrastructure;
using Common.Infrastructure.Modifiable;
using Common.Infrastructure.Observation;
using Common.Utility;
using Features.Localization.Data;
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
            int level,
            int unitCount,
            float unitHealth,
            float unitCombatStrength,
            ISampler hitSampler,
            Sprite commanderIcon,
            Sprite unitIcon,
            CombatantLocalizationResources combatantLoc)
        {
            Level = level;
            HitSampler = hitSampler;
            CommanderIcon = commanderIcon;
            UnitIcon = unitIcon;
            UnitName = combatantLoc.UnitName;
            Name = combatantLoc.TeamName;

            var loc = ResourceManager.Instance.LocalizationResources.Combat;

            var baseUnitHealth = new CombatBaseValue(unitHealth, loc.HealthPerUnit);
            var baseUnitStrength = new CombatBaseValue(unitCombatStrength, loc.StrengthPerUnit);

            UnitHealth = new ModifiableVariable(loc.HealthPerUnit, true, baseUnitHealth);
            UnitCombatStrength = new ModifiableVariable(loc.StrengthPerUnit, true, baseUnitStrength);

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