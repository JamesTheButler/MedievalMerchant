using System;
using UnityEngine;

namespace Features.Trade.Haggling.Data
{
    [Serializable]
    public sealed class HaggleConfigData
    {
        [field: SerializeField]
        public float CoinDifferencePercentage { get; private set; }

        [field: SerializeField]
        public float ReputationPer100Goods { get; private set; }
    }
}