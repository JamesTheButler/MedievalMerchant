using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Configuration
{
    /// <summary>
    /// Table to define which amount of different tier goods results in which development trend modifiers.
    /// </summary>
    [CreateAssetMenu(
        fileName = nameof(DevelopmentTable),
        menuName = AssetMenu.ConfigDataFolder + nameof(DevelopmentTable))]
    public sealed class DevelopmentTable : ScriptableObject
    {
        [field: SerializeField]
        public List<int> Tier1Trends { get; private set; }

        [field: SerializeField]
        public List<int> Tier2Trends { get; private set; }

        [field: SerializeField]
        public List<int> Tier3Trends { get; private set; }

        public int GetDevelopmentTrend(Tier goodTier, int goodCount)
        {
            return goodTier switch
            {
                Tier.Tier1 => GetTierTrend(goodCount, Tier1Trends), 
                Tier.Tier2 => GetTierTrend(goodCount, Tier2Trends),
                Tier.Tier3 => GetTierTrend(goodCount, Tier3Trends),
                _ => throw new ArgumentOutOfRangeException(nameof(goodTier), goodTier, null)
            };
        }

        private static int GetTierTrend(int goodAmount, List<int> trendsList)
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