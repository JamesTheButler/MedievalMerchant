using Common.Infrastructure.Gameplay;
using Common.Types;
using Features.Levels.Conditions.Model;

namespace Features.Levels.Conditions.Logic
{
    public sealed class TimeoutLossConditionLogic : IConditionLogic
    {
        private readonly TimeoutLossCondition _condition;
        private DateModel _gameDate;

        public TimeoutLossConditionLogic(TimeoutLossCondition condition)
        {
            _condition = condition;
        }

        public void Initialize()
        {
            _gameDate = GameplayContext.Instance.Model.DateModel;

            _gameDate.GameDate.Observe(DateChanged);
        }

        public void CleanUp()
        {
            _gameDate.GameDate.StopObserving(DateChanged);
        }
        
        private void DateChanged(Date date)
        {
            _condition.Progress.SetProgress(date.AsDays());
        }
    }
}