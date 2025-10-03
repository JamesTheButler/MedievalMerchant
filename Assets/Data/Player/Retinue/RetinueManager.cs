using System.Collections.Generic;
using Data.Player.Retinue.Config;
using Data.Player.Retinue.Logic;

namespace Data.Player.Retinue
{
    public class RetinueManager
    {
        CompanionConfig _companionConfig;

        public Dictionary<CompanionType, int> CompanionLevels { get; } = new();
        
        private Dictionary<CompanionType, ICompanionLogic> _companionLogics;

        public RetinueManager()
        {
        }
    }
}