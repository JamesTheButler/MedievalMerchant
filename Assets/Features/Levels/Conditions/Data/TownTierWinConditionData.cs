using System;
using Common.Types;
using Common.Utility;
using UnityEngine;

namespace Features.Levels.Conditions.Data
{
    [Serializable]
    public sealed class TownTierWinConditionData : WinConditionData
    {
        [field: SerializeField]
        public Tier TargetTier { get; private set; } = Tier.Tier3;

        [field: SerializeField]
        public int TargetCount { get; private set; } = 1;

        public override ConditionType Type => ConditionType.TownTierWinCondition;
        public override string Description => $"Develop {TargetCount} towns to Tier {TargetTier.ToRomanNumeral()}.";
    }
}