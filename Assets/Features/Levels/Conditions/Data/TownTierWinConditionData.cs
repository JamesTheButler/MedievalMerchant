using Common;
using Common.Types;
using UnityEngine;

namespace Features.Levels.Conditions.Data
{
    [CreateAssetMenu(
        fileName = nameof(TownTierWinConditionData),
        menuName = AssetMenu.ConditionsFolder + nameof(TownTierWinConditionData))]
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