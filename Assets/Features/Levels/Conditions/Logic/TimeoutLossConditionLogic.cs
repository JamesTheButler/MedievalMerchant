using Common.Infrastructure;
using Common.Types;
using Features.Levels.Conditions.Model;

namespace Features.Levels.Conditions.Logic
{
    public sealed class TimeoutLossConditionLogic : IConditionLogic
    {
        private readonly TimeoutLossCondition _condition;
        private Date _deadlineDate;
        private Date _currentDate;

        public TimeoutLossConditionLogic(TimeoutLossCondition condition)
        {
            _condition = condition;
        }

        public void Initialize()
        {
            _currentDate = GameplayContext.Instance.Model.Date;
            _deadlineDate = _condition.DeadlineDate;

            _currentDate.Day.Observe(DayChanged);
            _currentDate.Year.Observe(YearChanged);
        }

        public void CleanUp()
        {
            _currentDate.Day.StopObserving(DayChanged);
            _currentDate.Year.StopObserving(YearChanged);
        }

        private void YearChanged(int year)
        {
            Evaluate();
        }

        private void DayChanged(int day)
        {
            Evaluate();
        }

        private void Evaluate()
        {
            _condition.Progress.SetProgress(_currentDate.AsDays());
        }
    }
}