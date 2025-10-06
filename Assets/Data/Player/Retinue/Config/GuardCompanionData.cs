using System;
using System.Text;
using UnityEngine;

namespace Data.Player.Retinue.Config
{
    [Serializable]
    public sealed class GuardCompanionData : CompanionConfigData<GuardLevelData>
    {
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