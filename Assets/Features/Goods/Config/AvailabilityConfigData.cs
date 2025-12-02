using System;
using UnityEngine;

namespace Features.Goods.Config
{
    [Serializable]
    public sealed class AvailabilityConfigData
    {
        [field: SerializeField]
        public float PriceMultiplier { get; private set; }

        [field: SerializeField, Tooltip("Percentage of the max amount of the good.")]
        public float ActivationThresholdInPercent { get; private set; }
    }
}