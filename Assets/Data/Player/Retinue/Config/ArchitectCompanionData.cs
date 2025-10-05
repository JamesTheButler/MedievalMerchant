using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Data.Player.Retinue.Config
{
    [Serializable]
    public sealed class ArchitectCompanionData : CompanionConfigData
    {
        [field: SerializeField] public List<ArchitectLevelData> TypedLevels { get; private set; }
        public override IReadOnlyList<CompanionLevelData> Levels => TypedLevels;
    }

    [Serializable]
    public class ArchitectLevelData : CompanionLevelData
    {
        [field: SerializeField, Range(0f, 1f)] public float TownUpgradePriceReduction { get; private set; }
        [field: SerializeField, Range(0f, 1f)] public float ConstructionPriceReduction { get; private set; }

        public override string Description => new StringBuilder()
            .AppendLine($"-{ConstructionPriceReduction}% reduction to costs of production buildings")
            .AppendLine($"-{TownUpgradePriceReduction}% reduction of town upgrade costs")
            .ToString();
    }
}