using System;
using System.Text;
using Common;
using UnityEngine;

namespace Features.Player.Retinue.Config.LevelDatas
{
    [Serializable]
    public class NegotiatorLevelData : CompanionLevelData
    {
        [field: SerializeField]
        public float PriceSavings { get; private set; }

        [field: SerializeField]
        public float UpgradeCostReduction { get; private set; }

        public override string Description => new StringBuilder()
            .AppendLine($"- {PriceSavings.ToPercentString()} better prices")
            .AppendLine($"- {UpgradeCostReduction.ToPercentString()} reduction of caravan upgrade costs")
            .ToString();
    }
}