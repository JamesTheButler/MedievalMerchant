using Common.Types;
using Common.Utility;
using UnityEngine;

namespace Features.Levels.Conditions.Data
{
    [CreateAssetMenu(
        fileName = nameof(TimeoutLossConditionData),
        menuName = AssetMenu.ConditionsFolder + nameof(TimeoutLossConditionData))]
    public sealed class TimeoutLossConditionData : LossConditionData
    {
        [field: SerializeField, Min(1)]
        public int DeadlineYear { get; private set; } = 1;

        [field: SerializeField, Range(1, Date.LastDayOfYear)]
        public int DeadlineDay { get; private set; } = 1;

        public override ConditionType Type => ConditionType.TimeoutCondition;
        public override string Description => GetDescription();

        private string GetDescription()
        {
            return DeadlineDay switch
            {
                <= 1 => $"You lose at the start of year {DeadlineYear}",
                >= Date.LastDayOfYear => $"ou lose at the end of year {DeadlineYear}",
                _ => $"ou lose on day {DeadlineDay} of year {DeadlineYear}",
            };
        }
    }
}