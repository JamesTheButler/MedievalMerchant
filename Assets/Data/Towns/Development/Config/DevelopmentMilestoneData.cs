using System;
using Data.Towns.Development.Logic.Upgrades;
using NaughtyAttributes;
using UnityEngine;

namespace Data.Towns.Development.Config
{
    [Serializable]
    public sealed class DevelopmentMilestoneData
    {
        [field: SerializeField, Required, ShowAssetPreview(32,32)]
        public Sprite Icon { get; private set; }
        
        [field: SerializeField]
        public TownUpgradeData[] Upgrades { get; private set; }

    }
}