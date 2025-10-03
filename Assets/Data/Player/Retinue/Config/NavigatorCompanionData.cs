using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Player.Retinue.Config
{
    [Serializable]
    public sealed class NavigatorCompanionData : CompanionConfigData
    {
        [Serializable]
        public class LevelData
        {
            [field: SerializeField] public float Cost { get; private set; }
            [field: SerializeField] public float SpeedBonus { get; private set; }
            [field: SerializeField] public float UpkeepReduction { get; private set; }
        }

        [field: SerializeField] public List<LevelData> Levels { get; private set; }
    }
}