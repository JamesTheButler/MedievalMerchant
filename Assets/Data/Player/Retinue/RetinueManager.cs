using System;
using System.Collections.Generic;
using Common;
using Data.Configuration;
using Data.Player.Retinue.Logic;

namespace Data.Player.Retinue
{
    public sealed class RetinueManager
    {
        public Dictionary<CompanionType, Observable<int>> CompanionLevels { get; } = new();
        
        private readonly Dictionary<CompanionType, ICompanionLogic> _companionLogics;

        public RetinueManager()
        {
            var companionConfig = ConfigurationManager.Instance.CompanionConfig;
            
            foreach (CompanionType companionType in Enum.GetValues(typeof(CompanionType)))
            {
                CompanionLevels.Add(companionType, new Observable<int>());
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

        public void Upgrade(CompanionType companionType)
        {
            CompanionLevels[companionType].Value++;
            RefreshLogic(companionType);
        }
        
        public void Downgrade(CompanionType companionType)
        {
            CompanionLevels[companionType].Value--;
            RefreshLogic(companionType);
        }
        
        public void SetLevel(CompanionType companionType, int newLevel)
        {
            CompanionLevels[companionType].Value = newLevel;
            RefreshLogic(companionType);
        }

        private void RefreshLogic(CompanionType companionType)
        {
            var level = CompanionLevels[companionType].Value;
            _companionLogics[companionType].SetLevel(level);
        }
    }
}