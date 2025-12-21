using System;
using System.Collections.Generic;
using Common.Infrastructure.Modifiable;
using Common.Infrastructure.Observation;
using Features.Player.Caravan.Logic;

namespace Features.Player.Retinue.Logic
{
    public sealed class RetinueManager
    {
        public ModifiableVariable Upkeep { get; } = new("Retinue Upkeep", false);

        public Dictionary<CompanionType, Observable<int>> CompanionLevels { get; } = new();

        private readonly Dictionary<CompanionType, CompanionUpkeepModifier> _upkeepModifiers = new();

        private readonly Dictionary<CompanionType, ICompanionLogic> _companionLogics;

        public RetinueManager()
        {
            foreach (CompanionType companionType in Enum.GetValues(typeof(CompanionType)))
            {
                CompanionLevels.Add(companionType, new Observable<int>());
                var upkeepModifier = new CompanionUpkeepModifier(companionType);
                _upkeepModifiers.Add(companionType, upkeepModifier);
                Upkeep.AddModifier(upkeepModifier);
            }

            _companionLogics = new Dictionary<CompanionType, ICompanionLogic>
            {
                { CompanionType.Architect, new ArchitectCompanionLogic() },
                { CompanionType.Diplomat, new DiplomatCompanionLogic() },
                { CompanionType.Guard, new GuardCompanionLogic() },
                { CompanionType.Navigator, new NavigatorCompanionLogic() },
                { CompanionType.Negotiator, new NegotiatorCompanionLogic() },
                { CompanionType.Thief, new ThiefCompanionLogic() },
            };
        }

        public void SetLevel(CompanionType companionType, int newLevel)
        {
            CompanionLevels[companionType].Value = newLevel;
            var level = CompanionLevels[companionType].Value;
            _companionLogics[companionType].SetLevel(level);
            _upkeepModifiers[companionType].SetLevel(level);
        }
    }
}