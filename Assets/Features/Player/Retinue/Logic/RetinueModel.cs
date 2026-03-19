using System;
using System.Collections.Generic;
using Common.Infrastructure;
using Common.Infrastructure.Modifiable;

namespace Features.Player.Retinue.Logic
{
    public sealed class RetinueModel
    {
        public ModifiableVariable Upkeep { get; }

        public Dictionary<CompanionType, CompanionModel> Companions { get; } = new();

        public RetinueModel()
        {
            var loc = ResourceManager.Instance.LocalizationResources;
            Upkeep = new ModifiableVariable(loc.Player.RetinueUpkeep, false);
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