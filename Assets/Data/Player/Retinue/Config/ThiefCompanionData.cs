using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Data.Player.Retinue.Config
{
    [Serializable]
    public sealed class ThiefCompanionData : CompanionConfigData
    {
        [field: SerializeField] public List<ThiefLevelData> TypedLevels { get; private set; }
        public override IReadOnlyList<CompanionLevelData> Levels => TypedLevels;
    }

    [Serializable]
    public class ThiefLevelData : CompanionLevelData
    {
        [field: SerializeField] public float TownEntranceGold { get; private set; }
        [field: SerializeField, Range(0f, 1f)] public float ReputationLossChance { get; private set; }
        [field: SerializeField, Range(0, 100)] public float ReputationLoss { get; private set; }


        public override string Description => new StringBuilder()
            .AppendLine($"- Steals {TownEntranceGold} coin when entering town")
            .AppendLine($"- {ReputationLossChance.ToPercentString()} chance of getting caught")
            .AppendLine($"- {ReputationLoss} reputation lost when being caught")
            .ToString();
    }
}