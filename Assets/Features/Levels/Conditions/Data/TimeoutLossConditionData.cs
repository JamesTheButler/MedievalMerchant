using System;
using Common.Types;
using UnityEngine;

namespace Features.Levels.Conditions.Data
{
    [Serializable]
    public sealed class TimeoutLossConditionData : LossConditionData
    {
        [field: SerializeField, Min(1)]
        public int DeadlineYear { get; private set; } = 1;

        [field: SerializeField, Range(1, DateModel.LastDayOfYear)]
        public int DeadlineDay { get; private set; } = 1;

        public override ConditionType Type => ConditionType.TimeoutCondition;
        public override string Description => GetDescription();

        private string GetDescription()
        {
            return DeadlineDay switch
            {
                <= 1 => $"You lose at the start of year {DeadlineYear}",
                >= DateModel.LastDayOfYear => $"You lose at the end of year {DeadlineYear}",
                _ => $"You lose on day {DeadlineDay} of year {DeadlineYear}",
            };
        }
    }
}