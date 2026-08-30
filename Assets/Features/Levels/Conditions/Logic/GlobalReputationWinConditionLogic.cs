using Common.Infrastructure.Gameplay;
using Common.Infrastructure.Observation;
using Features.Levels.Conditions.Model;

namespace Features.Levels.Conditions.Logic
{
    public sealed class GlobalReputationWinConditionLogic : IConditionLogic
    {
        private readonly GlobalReputationWinCondition _condition;
        private readonly Bindings _bindings = new();

        private float _globalRepSum;
        private int _townCount;

        private GameplayModel _model;

        public GlobalReputationWinConditionLogic(GlobalReputationWinCondition condition)
        {
            _condition = condition;
        }

        public void Initialize()
        {
            _model = GameplayContext.Instance.Model;
            _townCount = _model.Towns.Count;

            foreach (var town in _model.Towns.Values)
            {
                _bindings.Track(
                    town.ReputationModel.Reputation.Observe(OnReputationChanged)
                );
                var repValue = town.ReputationModel.Reputation.Value;
                _globalRepSum += repValue;
            }
        }

        public void CleanUp()
        {
            _bindings.Unbind();
        }

        private void OnReputationChanged(float oldRep, float newRep)
        {
            _globalRepSum = _globalRepSum - oldRep + newRep;

            _condition.Progress.SetProgress((int)(_globalRepSum / _townCount));
        }
    }
}