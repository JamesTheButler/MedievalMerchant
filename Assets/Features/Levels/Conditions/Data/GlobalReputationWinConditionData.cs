using System;
using UnityEngine;

namespace Features.Levels.Conditions.Data
{
    [Serializable]
    public sealed class GlobalReputationWinConditionData : WinConditionData
    {
        [field: SerializeField]
        public int GlobalAverageReputation { get; private set; } = 50;

        public override ConditionType Type => ConditionType.GlobalRepWinCondition;

        public override string Description =>
            $"Reach an average of {GlobalAverageReputation} reputation across all towns.";
    }
}