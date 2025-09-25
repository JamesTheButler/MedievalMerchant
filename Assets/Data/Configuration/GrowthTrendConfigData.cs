using System;
using NaughtyAttributes;
using UnityEngine;

namespace Data.Configuration
{
    [Serializable]
    public sealed class GrowthTrendConfigData
    {
        [field: SerializeField, ShowAssetPreview]
        public Sprite Icon { get; private set; }

        [field: SerializeField]
        public float Threshold { get; private set; }
    }
}