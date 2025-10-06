using System;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

namespace Data.Player.Retinue.Config
{
    [Serializable]
    public sealed class ArchitectCompanionData : CompanionConfigData<ArchitectLevelData>
    {
    }

    [Serializable]
    public class ArchitectLevelData : CompanionLevelData
    {
        [field: SerializeField, Range(0f, 1f)] public float TownUpgradePriceReduction { get; private set; }
        [field: SerializeField, Range(0f, 1f)] public float ConstructionPriceReduction { get; private set; }

        public override string Description => new StringBuilder()
            .AppendLine($"- {ConstructionPriceReduction.ToPercentString()} reduction to costs of production buildings")
            .AppendLine($"- {TownUpgradePriceReduction.ToPercentString()} reduction of town upgrade costs")
            .ToString();
    }
}