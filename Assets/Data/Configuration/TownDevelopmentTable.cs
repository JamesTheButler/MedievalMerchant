using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Configuration
{
    /// <summary>
    /// Table to define which amount of different tier goods results in which development trend modifiers.
    /// </summary>
    [CreateAssetMenu(
        fileName = nameof(TownDevelopmentTable),
        menuName = AssetMenu.ConfigDataFolder + nameof(TownDevelopmentTable))]
    public sealed class TownDevelopmentTable : ScriptableObject
    {
        [field: SerializeField]
        public List<float> Tier1Trends { get; private set; }

        [field: SerializeField]
        public List<float> Tier2Trends { get; private set; }

        [field: SerializeField]
        public List<float> Tier3Trends { get; private set; }

        public float GetDevelopmentTrend(Tier goodTier, int goodCount)
        {
            return goodTier switch
            {
                Tier.Tier1 => GetTierTrend(goodCount, Tier1Trends), 
                Tier.Tier2 => GetTierTrend(goodCount, Tier2Trends),
                Tier.Tier3 => GetTierTrend(goodCount, Tier3Trends),
                _ => throw new ArgumentOutOfRangeException(nameof(goodTier), goodTier, null)
            };
        }

        private static float GetTierTrend(int goodAmount, List<float> trendsList)
        {
            if (trendsList.Count == 0)
                return 0;

            if (goodAmount <= 0)
                return trendsList[0];

            if (goodAmount > trendsList.Count)
                return trendsList[^1] * (goodAmount - trendsList.Count + 1);

            return trendsList[goodAmount];
        }
    }
}