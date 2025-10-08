using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Player.Retinue.Config
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