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

        private Date _date;

        public override ConditionType Type => ConditionType.DateReachedLossCondition;

        public override void Initialize()
        {
            _date = Model.Instance.Date;
            _date.Day.Observe(DayChanged);
            _date.Year.Observe(YearChanged);
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
            var day = _date.Day.Value;
            var year = _date.Year.Value;

            if (year > deadlineYear || (year == deadlineYear && day > deadlineDay))
            {
                IsCompleted.Value = true;

                _date.Day.StopObserving(DayChanged);
                _date.Year.StopObserving(YearChanged);
            }
        }
    }
}