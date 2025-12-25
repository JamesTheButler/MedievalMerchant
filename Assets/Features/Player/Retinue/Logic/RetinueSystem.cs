using System.Collections.Generic;
using Common.Infrastructure;
using Features.Player.Retinue.Logic.CompanionLogics;

namespace Features.Player.Retinue.Logic
{
    public sealed class RetinueSystem : ISystem
    {
        private Dictionary<CompanionType, ICompanionLogic> _companionLogics;

        private RetinueModel _retinueModel;

        public void Initialize()
        {
            _retinueModel = GameplayContext.Instance.Model.Player.RetinueModel;

            _companionLogics = new Dictionary<CompanionType, ICompanionLogic>
            {
                { CompanionType.Architect, new ArchitectCompanionLogic() },
                { CompanionType.Diplomat, new DiplomatCompanionLogic() },
                { CompanionType.Guard, new GuardCompanionLogic() },
                { CompanionType.Navigator, new NavigatorCompanionLogic() },
                { CompanionType.Negotiator, new NegotiatorCompanionLogic() },
                { CompanionType.Thief, new ThiefCompanionLogic() },
            };

            foreach (var (companion, level) in _retinueModel.CompanionLevels)
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