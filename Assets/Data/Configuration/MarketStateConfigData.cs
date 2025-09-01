using System;
using UnityEngine;

namespace Data.Configuration
{
    [Serializable]
    public sealed class MarketStateConfigData
    {
        [field: SerializeField]
        public Sprite Icon { get; private set; }
        
        [field: SerializeField]
        public string DisplayString { get; private set; }

        [field: SerializeField]
        public float PriceMultiplier { get; private set; }

        [field: SerializeField]
        public float ActivationThreshold { get; private set; }
    }
}