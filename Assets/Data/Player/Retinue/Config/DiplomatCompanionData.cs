using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Player.Retinue.Config
{
    [Serializable]
    public sealed class DiplomatCompanionData : CompanionConfigData
    {
        [Serializable]
        public class LevelData
        {
            [field: SerializeField] public float Cost { get; private set; }
            [field: SerializeField, Range(0, 100)] public float TownEntranceReputation { get; private set; }
            [field: SerializeField] public float ReputationBoost { get; private set; }
        }

        [field: SerializeField] public List<LevelData> Levels { get; private set; }
    }
}