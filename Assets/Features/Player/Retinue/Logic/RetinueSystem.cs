using System.Collections.Generic;
using Common.Infrastructure;

namespace Features.Player.Retinue.Logic
{
    public sealed class RetinueSystem : ISystem
    {
        private Dictionary<CompanionType, ICompanionLogic> _companionLogics;

        private RetinueManager _retinueManager;

        public void Initialize()
        {
            _retinueManager = GameplayContext.Instance.Model.Player.RetinueManager;

            _companionLogics = new Dictionary<CompanionType, ICompanionLogic>
            {
                { CompanionType.Architect, new ArchitectCompanionLogic() },
                { CompanionType.Diplomat, new DiplomatCompanionLogic() },
                { CompanionType.Guard, new GuardCompanionLogic() },
                { CompanionType.Navigator, new NavigatorCompanionLogic() },
                { CompanionType.Negotiator, new NegotiatorCompanionLogic() },
                { CompanionType.Thief, new ThiefCompanionLogic() },
            };

            foreach (var (companion, level) in _retinueManager.CompanionLevels)
            {
                level.Observe(lvl => UpdateLevel(companion, lvl));
            }
        }

        public void CleanUp() { }

        private void UpdateLevel(CompanionType companion, int level)
        {
            _companionLogics[companion].SetLevel(level);
        }
    }
}