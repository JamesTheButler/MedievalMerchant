using System;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Goods.Config
{
    [Serializable]
    public sealed class AvailabilityConfigData
    {
        [field: SerializeField, ShowAssetPreview]
        public Sprite Icon { get; private set; }
        
        [field: SerializeField]
        public string DisplayString { get; private set; }

        [field: SerializeField]
        public float PriceMultiplier { get; private set; }

        [field: SerializeField, Tooltip("Percentage of the max amount of the good.")]
        public float ActivationThresholdInPercent { get; private set; }
    }
}