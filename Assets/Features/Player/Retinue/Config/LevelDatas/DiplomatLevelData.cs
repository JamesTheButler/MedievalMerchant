using System;
using System.Text;
using Common;
using UnityEngine;

namespace Features.Player.Retinue.Config.LevelDatas
{
    [Serializable]
    public class DiplomatLevelData : CompanionLevelData
    {
        [field: SerializeField, Range(0, 100)]
        public float TownEntranceReputation { get; private set; }

        [field: SerializeField]
        public float ReputationBoost { get; private set; }

        public override string Description => new StringBuilder()
            .AppendLine($"- +{TownEntranceReputation} reputation when entering town")
            .AppendLine($"- {ReputationBoost.ToPercentString()} bonus for all reputation gains")
            .ToString();
    }
}