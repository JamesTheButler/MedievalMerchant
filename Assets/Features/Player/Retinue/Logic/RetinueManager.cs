using System;
using System.Collections.Generic;
using Common.Infrastructure.Modifiable;
using Common.Infrastructure.Observation;

namespace Features.Player.Retinue.Logic
{
    public sealed class RetinueManager
    {
        public ModifiableVariable Upkeep { get; } = new("Retinue Upkeep", false);

        public Dictionary<CompanionType, Observable<int>> CompanionLevels { get; } = new();

        private readonly Dictionary<CompanionType, CompanionUpkeepModifier> _upkeepModifiers = new();

        public RetinueManager()
        {
            foreach (CompanionType companionType in Enum.GetValues(typeof(CompanionType)))
            {
                CompanionLevels.Add(companionType, new Observable<int>());
                var upkeepModifier = new CompanionUpkeepModifier(companionType);
                _upkeepModifiers.Add(companionType, upkeepModifier);
                Upkeep.AddModifier(upkeepModifier);
            }
        }

        public void SetLevel(CompanionType companionType, int newLevel)
        {
            CompanionLevels[companionType].Value = newLevel;
            var level = CompanionLevels[companionType].Value;
            _upkeepModifiers[companionType].SetLevel(level);
        }
    }
}