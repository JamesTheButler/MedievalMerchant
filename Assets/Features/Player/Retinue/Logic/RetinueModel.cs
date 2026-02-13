using System;
using System.Collections.Generic;
using Common.Infrastructure.Modifiable;
using Features.Player.Retinue.Logic.Modifiers;

namespace Features.Player.Retinue.Logic
{
    public sealed class RetinueModel
    {
        public ModifiableVariable Upkeep { get; } = new("Retinue Upkeep", false);

        public Dictionary<CompanionType, CompanionModel> Companions { get; } = new();

        private readonly Dictionary<CompanionType, CompanionUpkeepModifier> _upkeepModifiers = new();

        public RetinueModel()
        {
            foreach (CompanionType companionType in Enum.GetValues(typeof(CompanionType)))
            {
                Companions.Add(companionType, new CompanionModel(companionType));
                var upkeepModifier = new CompanionUpkeepModifier(companionType);
                _upkeepModifiers.Add(companionType, upkeepModifier);
                Upkeep.AddModifier(upkeepModifier);
            }
        }

        public void SetLevel(CompanionType companionType, int newLevel)
        {
            Companions[companionType].SetLevel(newLevel);
            _upkeepModifiers[companionType].SetLevel(newLevel);
        }
    }
}