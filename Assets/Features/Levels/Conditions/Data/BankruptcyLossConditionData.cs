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

        public override ConditionType Type => ConditionType.BankruptcyLossCondition;

        public override string Description => "Don't run out of coin!";
    }
}