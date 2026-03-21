using System;
using UnityEngine;

namespace Features.Levels.Conditions.Data
{
    [Serializable]
    public sealed class BankruptcyLossConditionData : LossConditionData
    {
        [field: SerializeField]
        public int MaxBankruptcyDurationInDays { get; private set; } = 7;

        [field: SerializeField]
        public int BankruptcyFundsThreshold { get; private set; }

        [field: SerializeField]
        public int DaysLeftThreshold { get; private set; } = 4;

        public override ConditionType Type => ConditionType.BankruptcyLossCondition;

        public override string Description => formatter.GetLocalizedString();
        public override string WarningMessage => warningMessageFormatter.GetLocalizedString(DaysLeftThreshold);
        public override string GameOverMessage => gameOverMessageFormatter.GetLocalizedString(MaxBankruptcyDurationInDays);
    }
}