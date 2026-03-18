using System;
using System.Collections.Generic;
using Features.Player.Retinue.Config.LevelDatas;
using UnityEngine;

namespace Features.Player.Retinue.Config.CompanionDatas
{
    [Serializable]
    public abstract class CompanionConfigData<T> : CompanionConfigData
        where T : CompanionLevelData
    {
        [SerializeField]
        private List<T> typedLevels;

        public override IReadOnlyList<CompanionLevelData> Levels => typedLevels;

        public T GetTypedLevelData(int level)
        {
            if (level <= 0 || level > Levels.Count)
                return null;

            return typedLevels[level - 1];
        }
    }
}