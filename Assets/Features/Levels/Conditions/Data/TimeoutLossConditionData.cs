using Common;
using Common.Types;
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
                <= 1 => $"Win the game by the start of Year {DeadlineYear}",
                >= Date.LastDayOfYear => $"Win the game by the end of Year {DeadlineYear}",
                _ => $"Win the game by Day {DeadlineDay} of Year {DeadlineYear}",
            };
        }
    }
}