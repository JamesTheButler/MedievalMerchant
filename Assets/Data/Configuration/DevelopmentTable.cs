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

        public int GetDevelopmentTrend(int tier1Goods, int tier2Goods, int tier3Goods)
        {
            return
                GetTierTrend(tier1Goods, Tier1Trends) +
                GetTierTrend(tier2Goods, Tier2Trends) +
                GetTierTrend(tier3Goods, Tier3Trends);
        }

        private int GetTierTrend(int goodAmount, List<int> trendsList)
        {
            if (trendsList.Count == 0)
                return 0;

            if (goodAmount <= 0)
                return trendsList[0];

            if (goodAmount > trendsList.Count)
                return trendsList[^1] * (goodAmount - trendsList.Count + 1);

            return trendsList[goodAmount - 1];
        }
    }
}