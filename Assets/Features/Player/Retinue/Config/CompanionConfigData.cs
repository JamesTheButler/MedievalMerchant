using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Player.Retinue.Config
{
    [Serializable]
    public abstract class CompanionConfigData
    {
        [field: SerializeField, Required, ShowAssetPreview]
        public Sprite Icon { get; private set; }

        [field: SerializeField]
        public string Name { get; private set; }

        [field: SerializeField]
        public string Description { get; private set; }

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

        public string DisplayString(int level)
        {
            var comingSoonSuffix = IsImplemented ? string.Empty : " - (coming soon)";
            return $"{Name} lvl. {level} {comingSoonSuffix}";
        }
    }
}