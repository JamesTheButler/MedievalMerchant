using System;
using System.Collections.Generic;
using System.Linq;
using Common.Infrastructure.Modifiable;
using Common.Infrastructure.Observation;

namespace Features.Combat
{
    public sealed class Combatant
    {
        private readonly Func<float> _hitFactorFunc;
        private readonly ObservableSum _totalStrength = new();

        public Observable<float> TotalStrength => _totalStrength;
        public Observable<float> TotalHealth { get; }
        public Observable<int> AliveUnitCount { get; }

        private readonly List<CombatUnit> _units, _aliveUnits;

        public IReadOnlyList<CombatUnit> Units => _units;
        public IReadOnlyList<CombatUnit> AliveUnits => _aliveUnits;

        public Combatant(
            int unitCount,
            float unitHealth,
            float unitStrength,
            Func<float> hitFactorFunc)
        {
            _hitFactorFunc = hitFactorFunc;
            _units = new List<CombatUnit>(unitCount);
            _aliveUnits = new List<CombatUnit>(unitCount);
            for (var i = 0; i < unitCount; i++)
            {
                var unit = new CombatUnit(unitHealth, unitStrength);
                unit.IsAlive.Observe(isAlive => OnUnitAliveChanged(unit, isAlive), false);
                _units.Add(unit);
                _aliveUnits.Add(unit);
                _totalStrength.AddValue(unit.Strength);
            }

            TotalHealth = new ObservableSum(_units.Select(unit => unit.Health));
            AliveUnitCount = new ObservableFilter<bool>(
                _units.Select(unit => unit.IsAlive),
                value => value);
        }

        public float HitFactor()
        {
            return _hitFactorFunc.Invoke();
        }

        private void OnUnitAliveChanged(CombatUnit unit, bool isAlive)
        {
            if (isAlive)
                return;

            _aliveUnits.Remove(unit);
            _totalStrength.RemoveValue(unit.Strength);
        }
    }
}