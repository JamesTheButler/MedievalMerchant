using Common;
using Common.Modifiable;
using Features.Towns;
using Features.Towns.Development.Logic.Milestones;

namespace Features.Player
{
    public sealed class DividendsSystem : IService
    {
        private GameplayModel _model;
        private PlayerModel _player;

        public void Initialize()
        {
            _model = GameplayModel.Instance;
            _player = _model.Player;
            foreach (var town in _model.Towns.Values)
            {
                ObserveTown(town);
            }
        }

        public void CleanUp()
        {
            foreach (var town in _model.Towns.Values)
            {
                UnobserveTown(town);
            }
        }

        private void ObserveTown(Town town)
        {
            town.MilestoneManager.MilestoneModifierAdded += OnMilestoneAdded;
            town.MilestoneManager.MilestoneModifierRemoved += OnMilestoneRemoved;
        }

        private void UnobserveTown(Town town)
        {
            town.MilestoneManager.MilestoneModifierAdded -= OnMilestoneAdded;
            town.MilestoneManager.MilestoneModifierRemoved -= OnMilestoneRemoved;
        }

        private void OnMilestoneAdded(IModifier modifier)
        {
            if (modifier is not DividendsFundsModifier dividendsModifier)
                return;
            
            _player.FundsChange.AddModifier(dividendsModifier);
        }

        private void OnMilestoneRemoved(IModifier modifier)
        {
            if (modifier is not DividendsFundsModifier dividendsModifier)
                return;
            
            _player.FundsChange.RemoveModifier(dividendsModifier);
        }
    }
}