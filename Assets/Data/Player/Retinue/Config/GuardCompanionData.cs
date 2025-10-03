using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Player.Retinue.Config
{
    [Serializable]
    public sealed class GuardCompanionData : CompanionConfigData
    {
        [Serializable]
        public class LevelData
        {
            [field: SerializeField] public float Cost { get; private set; }
            [field: SerializeField] public int Strength { get; private set; }
        }

        [field: SerializeField] public List<LevelData> Levels { get; private set; }
    }
}