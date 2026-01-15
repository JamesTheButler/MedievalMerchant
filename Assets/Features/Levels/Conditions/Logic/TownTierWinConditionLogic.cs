using System.Linq;
using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Types;
using Features.Levels.Conditions.Model;

namespace Features.Levels.Conditions.Logic
{
    public sealed class TownTierWinConditionLogic : IConditionLogic
    {
        private readonly TownTierWinCondition _condition;
        private GameplayModel _model;

        public TownTierWinConditionLogic(TownTierWinCondition condition)
        {
            _condition = condition;
        }

        public void Initialize()
        {
            _model = GameplayContext.Instance.Model;
            foreach (var town in _model.Towns.Values)
            {
                town.Tier.Observe(OnTierChanged);
            }
        }

        public void CleanUp()
        {
            foreach (var town in _model.Towns.Values)
            {
                town.Tier.StopObserving(OnTierChanged);
            }
        }

        private void OnTierChanged(Tier tier)
        {
            var currentCount = _model.Towns.Values.Count(town => town.Tier.Value >= _condition.TargetTier);
            _condition.Progress.SetProgress(currentCount);
        }
    }
}