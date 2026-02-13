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

        public RetinueModel()
        {
            foreach (CompanionType companionType in Enum.GetValues(typeof(CompanionType)))
            {
                var companionModel = new CompanionModel(companionType);
                Companions.Add(companionType, companionModel);
                Upkeep.AddModifier(companionModel.UpkeepModifier);
            }
        }

        public void SetLevel(CompanionType companionType, int newLevel)
        {
            Companions[companionType].SetLevel(newLevel);
        }
    }
}