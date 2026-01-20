using System;
using UnityEngine;

namespace Features.Goods.Config
{
    [Serializable]
    public sealed class GlobalSurplusModiferConfigData
    {
        /// <summary>
        /// How many goods can be in the global market before the modifier kicks in.
        /// </summary>
        [field: SerializeField]
        public int StartThreshold { get; private set; } = 100;

        /// <summary>
        /// How many goods need to be in global surplus for each step change in the price modifier.
        /// </summary>
        [field: SerializeField]
        public int GoodsPerStep { get; private set; } = 100;

        /// <summary>
        /// How large is the percentage change to prices, per step (i.e. per GoodsPerStep goods)
        /// </summary>
        [field: SerializeField, Range(-1f, 0f)]
        public float PriceReductionPerStep { get; private set; } = -0.01f;
    }
}