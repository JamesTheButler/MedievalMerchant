using System;
using UnityEngine;

namespace Features.Levels.Conditions.Data
{
    [Serializable]
    public sealed class FundsWinConditionData : WinConditionData
    {
        [field: SerializeField]
        public int FundsToReach { get; private set; } = 9999;

        public override ConditionType Type => ConditionType.FundsWinCondition;
        public override string Description => formatter.GetLocalizedString(FundsToReach);
    }
}