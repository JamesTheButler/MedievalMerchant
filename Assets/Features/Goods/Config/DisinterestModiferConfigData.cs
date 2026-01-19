using System;
using UnityEngine;

namespace Features.Goods.Config
{
    [Serializable]
    public sealed class DisinterestModiferConfigData
    {
        [field: SerializeField]
        public int TrackedPeriodInDays { get; private set; } = 30;

        [field: SerializeField]
        public int GoodsPerStep { get; private set; } = 100;

        [field: SerializeField]
        public float PriceReductionPerStep { get; private set; } = 0.01f;
    }
}