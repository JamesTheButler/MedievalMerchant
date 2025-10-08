using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Data.Player.Retinue.Config
{
    [Serializable]
    public abstract class CompanionConfigData<T> : CompanionConfigData
        where T : CompanionLevelData
    {
        [SerializeField]
        private List<T> TypedLevels;

        public T GetLevelData(int level)
        {
            if (level <= 0 || level > Levels.Count)
                return null;
            return TypedLevels[level - 1];
        }

        public override IReadOnlyList<CompanionLevelData> Levels => TypedLevels;
    }
}