using Data;
using Data.Configuration;
using UnityEngine;

namespace Levels.Conditions
{
    [CreateAssetMenu(
        fileName = nameof(DateReachedLossCondition),
        menuName = AssetMenu.ConditionsFolder + nameof(DateReachedLossCondition))]
    public sealed class DateReachedLossCondition : LossCondition
    {
        [SerializeField]
        private int deadlineYear;

        [SerializeField]
        private int deadlineDay;

        private Date _deadlineDate;
        private Date _currentDate;

        public override ConditionType Type => ConditionType.DateReachedLossCondition;

        public override string Description => GetDescription();


        public override void Initialize()
        {
            _currentDate = Model.Instance.Date;
            _deadlineDate = new Date(deadlineDay, deadlineYear);

            Progress = new Progress(_deadlineDate.AsDays(), FormatProgress);

            _currentDate.Day.Observe(DayChanged);
            _currentDate.Year.Observe(YearChanged);
        }

        private void YearChanged(int year)
        {
            Evaluate();
        }

        private void DayChanged(int obj)
        {
            Evaluate();
        }

        private void Evaluate()
        {
            Progress.SetProgress(_currentDate.AsDays());
        }

        private string GetDescription()
        {
            return deadlineDay switch
            {
                1 => $"Win the game by the start of Year {deadlineYear}",
                Date.LastDayOfYear => $"Win the game by the end of Year {deadlineYear}",
                _ => $"Win the game by Day {deadlineDay} of Year {deadlineYear}",
            };
        }

        private static string FormatProgress(int currentValue, int maxValue)
        {
            var daysLeft = maxValue - currentValue;
            return $"{daysLeft} days left";
        }
    }
}