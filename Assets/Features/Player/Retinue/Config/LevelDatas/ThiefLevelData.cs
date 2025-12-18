using System;
using System.Text;
using Common.UI.Utility;
using Common.Utility;
using UnityEngine;

namespace Features.Player.Retinue.Config.LevelDatas
{
    [Serializable]
    public class ThiefLevelData : CompanionLevelData
    {
        [field: SerializeField]
        public float TownEntranceGold { get; private set; }

        [field: SerializeField, Range(0f, 1f)]
        public float ReputationLossChance { get; private set; }

        [field: SerializeField, Range(0, 100)]
        public float ReputationLoss { get; private set; }


        public override string Description => new StringBuilder()
            .AppendLine($"- Steals {TownEntranceGold} coin when entering town".WithGoodStyle())
            .AppendLine($"- {ReputationLossChance.ToPercentString()} chance of getting caught".WithBadStyle())
            .AppendLine($"- {ReputationLoss} reputation lost when being caught".WithBadStyle())
            .ToString();
    }
}