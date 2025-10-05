using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Data.Player.Retinue.Config
{
    [Serializable]
    public sealed class NavigatorCompanionData : CompanionConfigData
    {
        [field: SerializeField] public List<NavigatorLevelData> TypedLevels { get; private set; }
        public override IReadOnlyList<CompanionLevelData> Levels => TypedLevels;
    }

    [Serializable]
    public class NavigatorLevelData : CompanionLevelData
    {
        [field: SerializeField] public float SpeedBonus { get; private set; }
        [field: SerializeField] public float UpkeepReduction { get; private set; }

        public override string Description => new StringBuilder()
            .AppendLine($"- {SpeedBonus.ToPercentString()} shorter travel times ")
            .AppendLine($"- {UpkeepReduction.ToPercentString()} lower caravan upkeep")
            .ToString();
    }
}