using System;
using UnityEngine;

namespace Data.Configuration
{
    [Serializable]
    public sealed class GrowthTrendConfigData
    {
        [field: SerializeField]
        public Sprite Icon { get; private set; }

        [field: SerializeField]
        public float Threshold { get; private set; }
    }
}