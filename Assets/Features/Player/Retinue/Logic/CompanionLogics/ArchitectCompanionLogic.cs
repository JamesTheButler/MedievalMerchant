using Common.Infrastructure.Gameplay;
using Common.Types;
using Features.Player.Retinue.Config.CompanionDatas;
using Features.Player.Retinue.Logic.Modifiers;

namespace Features.Player.Retinue.Logic.CompanionLogics
{
    public sealed class ArchitectCompanionLogic : BaseCompanionLogic<ArchitectCompanionData>
    {
        protected override CompanionType Type => CompanionType.Architect;

        private readonly ArchitectUpgradeCostModifier _modifier;
        private readonly DateModel _gameDateModel;

        public ArchitectCompanionLogic()
        {
            _modifier = new ArchitectUpgradeCostModifier(0);
            var gameModel = GameplayContext.Instance.Model;
            foreach (var town in gameModel.Towns.Values)
            {
                town.ProductionManager.AddConstructionModifier(_modifier);
            }
        }

        public override void SetLevel(int level)
        {
            if (level <= 0)
                return;

            _modifier.Update(level);
        }
    }
}