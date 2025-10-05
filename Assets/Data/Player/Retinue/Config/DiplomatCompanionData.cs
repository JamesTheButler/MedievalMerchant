using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Data.Player.Retinue.Config
{
    [Serializable]
    public sealed class DiplomatCompanionData : CompanionConfigData
    {
        [field: SerializeField] public List<DiplomatLevelData> TypedLevels { get; private set; }
        public override IReadOnlyList<CompanionLevelData> Levels => TypedLevels;
    }

    [Serializable]
    public class DiplomatLevelData : CompanionLevelData
    {
        [field: SerializeField, Range(0, 100)] public float TownEntranceReputation { get; private set; }
        [field: SerializeField] public float ReputationBoost { get; private set; }

        public override string Description => new StringBuilder()
            .AppendLine($"- +{TownEntranceReputation} reputation when entering town")
            .AppendLine($"- {ReputationBoost.ToPercentString()} bonus for all reputation gains")
            .ToString();
    }
}