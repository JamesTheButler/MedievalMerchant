using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Data.Player.Retinue.Config
{
    [Serializable]
    public sealed class GuardCompanionData : CompanionConfigData
    {
        [field: SerializeField] public List<GuardLevelData> TypedLevels { get; private set; }
        public override IReadOnlyList<CompanionLevelData> Levels => TypedLevels;
    }

    [Serializable]
    public class GuardLevelData : CompanionLevelData
    {
        [field: SerializeField] public int Strength { get; private set; }

        public override string Description => new StringBuilder()
            .AppendLine($"- {Strength} combat strength")
            .ToString();
    }
}