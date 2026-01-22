using System.Collections.Generic;
using System.Linq;
using Common.Infrastructure.Gameplay;
using Common.Infrastructure.Observation;
using Features.Levels.Conditions.Model;
using Features.Towns;

namespace Features.Levels.Conditions.Logic
{
    public sealed class LocalReputationWinConditionLogic : IConditionLogic
    {
        private readonly LocalReputationWinCondition _condition;
        private readonly Bindings _bindings = new();
        private readonly HashSet<Town> _completedTowns = new();

        private GameplayModel _model;

        public LocalReputationWinConditionLogic(LocalReputationWinCondition condition)
        {
            _condition = condition;
        }

        public void Initialize()
        {
            _model = GameplayContext.Instance.Model;
            foreach (var town in _model.Towns.Values)
            {
                _bindings.Track(
                    town.ReputationModel.Reputation.Observe(Callback)
                );
                continue;

                void Callback(float oldRep, float newRep) => OnReputationChanged(town, oldRep, newRep);
            }
        }

        public void CleanUp()
        {
            _bindings.UnbindAll();
        }

        private void OnReputationChanged(Town town, float oldRep, float newRep)
        {
            var isOldOverThreshold = oldRep >= _condition.Reputation;
            var isNewOverThreshold = newRep >= _condition.Reputation;

            switch (Old: isOldOverThreshold, New: isNewOverThreshold)
            {
                case (Old: true, New: true):
                case (Old: false, New: false):
                    return;
                case (Old: false, New: true):
                    _completedTowns.Add(town);
                    break;
                case (Old: true, New: false):
                    _completedTowns.Remove(town);
                    break;
            }

            _condition.Progress.SetProgress(_completedTowns.Count);
        }
    }
}