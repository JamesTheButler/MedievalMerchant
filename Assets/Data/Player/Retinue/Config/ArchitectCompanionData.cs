using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Player.Retinue.Config
{
    [Serializable]
    public sealed class ArchitectCompanionData : CompanionConfigData
    {
        [Serializable]
        public class LevelData
        {
            [field: SerializeField] public float Cost { get; private set; }
            [field: SerializeField, Range(0f, 1f)] public float TownUpgradePriceReduction { get; private set; }
            [field: SerializeField, Range(0f, 1f)] public float ConstructionPriceReduction { get; private set; }
        }

        [field: SerializeField] public List<LevelData> Levels { get; private set; }
    }
}