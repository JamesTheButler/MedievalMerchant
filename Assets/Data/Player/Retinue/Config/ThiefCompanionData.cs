using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Player.Retinue.Config
{
    [Serializable]
    public sealed class ThiefCompanionData : CompanionConfigData
    {
        [Serializable]
        public class LevelData
        {
            [field: SerializeField] public float Cost { get; private set; }
            [field: SerializeField] public float TownEntranceGold { get; private set; }
            [field: SerializeField, Range(0f, 1f)] public float ReputationLossChance { get; private set; }
            [field: SerializeField, Range(0, 100)] public float ReputationLoss { get; private set; }
        }

        [field: SerializeField] public List<LevelData> Levels { get; private set; }
    }
}