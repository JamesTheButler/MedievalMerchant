using System;
using UnityEngine;

namespace Features.Player.Retinue.Config.LevelDatas
{
    [Serializable]
    public abstract class CompanionLevelData
    {
        [field: SerializeField]
        public float Upkeep { get; private set; }

        public abstract string Description { get; }
    }
}