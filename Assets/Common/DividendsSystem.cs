using Common.Modifiable;
using Features.Player;
using Features.Towns.Development.Logic.Milestones;
using Infrastructure;

namespace Common
{
    public sealed class DividendsSystem : ISystem
    {
        private PlayerModel _player;

        public void Initialize()
        {
            _player = GameplayModel.Instance.Player;
            foreach (var town in GameplayModel.Instance.Towns.Values)
            {
                town.MilestoneManager.MilestoneModifierAdded += OnMilestoneModifierAdded;
                town.MilestoneManager.MilestoneModifierRemoved += OnMilestoneModifierRemoved;
            }
        }

        public void CleanUp()
        {
            foreach (var town in GameplayModel.Instance.Towns.Values)
            {
                town.MilestoneManager.MilestoneModifierAdded += OnMilestoneModifierAdded;
                town.MilestoneManager.MilestoneModifierRemoved += OnMilestoneModifierRemoved;
            }
        }

        private void OnMilestoneModifierRemoved(IModifier modifier)
        {
            if (modifier is not DividendsFundsModifier dividendsFundsModifier)
                return;

            _player.FundsChange.AddModifier(dividendsFundsModifier);
        }

        private void OnMilestoneModifierAdded(IModifier modifier)
        {
            if (modifier is not DividendsFundsModifier dividendsFundsModifier)
                return;

            _player.FundsChange.RemoveModifier(dividendsFundsModifier);
        }
    }
}