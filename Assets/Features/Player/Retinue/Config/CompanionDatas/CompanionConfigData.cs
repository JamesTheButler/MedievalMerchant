using System;
using System.Collections.Generic;
using Features.Player.Retinue.Config.LevelDatas;
using UnityEngine;

namespace Features.Player.Retinue.Config.CompanionDatas
{
    [Serializable]
    public abstract class CompanionConfigData
    {
        [field: SerializeField]
        public bool IsImplemented { get; private set; }

        [field: SerializeField]
        public CompanionMissionConfig MissionConfig { get; private set; }

        public abstract IReadOnlyList<CompanionLevelData> Levels { get; }

        public int MaxLevel => Levels.Count;

        public CompanionLevelData GetLevelData(int level)
        {
            if (level <= 0 || level > Levels.Count)
                return null;

            return Levels[level - 1];
        }
    }
}