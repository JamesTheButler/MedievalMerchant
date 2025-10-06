using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Data.Player.Retinue.Config
{
    [Serializable]
    public abstract class CompanionConfigData<TLevelData>
        where TLevelData : CompanionLevelData
    {
        [field: SerializeField] public CompanionType Type { get; private set; }

        [field: SerializeField, Required, ShowAssetPreview]
        public Sprite Icon { get; private set; }

        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set; }

        [field: SerializeField] public List<TLevelData> TypedLevels { get; private set; }
    }
}