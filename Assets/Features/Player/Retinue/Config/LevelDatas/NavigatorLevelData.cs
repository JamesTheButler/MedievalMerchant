using System;
using System.Text;
using Common;
using UnityEngine;

namespace Features.Player.Retinue.Config.LevelDatas
{
    [Serializable]
    public class NavigatorLevelData : CompanionLevelData
    {
        [field: SerializeField]
        public float SpeedBonus { get; private set; }

        [field: SerializeField]
        public float UpkeepReduction { get; private set; }

        public override string Description => new StringBuilder()
            .AppendLine($"- {SpeedBonus.ToPercentString()} shorter travel times ")
            .AppendLine($"- {UpkeepReduction.ToPercentString()} lower caravan upkeep")
            .ToString();
    }
}