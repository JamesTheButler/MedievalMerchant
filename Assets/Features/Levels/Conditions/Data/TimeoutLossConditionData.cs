using System;
using UnityEngine;

namespace Features.Levels.Conditions.Data
{
    [Serializable]
    public sealed class TimeoutLossConditionData : LossConditionData
    {
        [field: SerializeField, Min(1)]
        public int DeadlineYear { get; private set; } = 1;

        [field: SerializeField]
        public int DaysLeftWarning { get; private set; } = 7;

        public override ConditionType Type => ConditionType.TimeoutCondition;
        public override string Description => formatter.GetLocalizedString(DeadlineYear);
        public override string WarningMessage => warningMessageFormatter.GetLocalizedString(DaysLeftWarning);
        public override string GameOverMessage => gameOverMessageFormatter.GetLocalizedString(DeadlineYear);
    }
}