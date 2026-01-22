using System;
using System.Text;
using Common.UI.Utility;
using Common.Utility;
using UnityEngine;

namespace Features.Player.Retinue.Config.LevelDatas
{
    [Serializable]
    public class ArchitectLevelData : CompanionLevelData
    {
        [field: SerializeField, Range(0f, 1f)]
        public float ConstructionPriceReduction { get; private set; }

        public override string Description => new StringBuilder()
            .AppendLine($"- {ConstructionPriceReduction.ToPercentString()} reduction to costs of production buildings")
            .ToString()
            .WithStyle(Style.Good);
    }
}