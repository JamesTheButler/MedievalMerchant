using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Data.Configuration
{
    [CreateAssetMenu(
        fileName = nameof(ProducerConfig),
        menuName = AssetMenu.ConfigDataFolder + nameof(ProducerConfig))]
    public sealed class ProducerConfig : ScriptableObject
    {
        [Header("Production Limits")]
        [SerializeField, SerializedDictionary("Good Tier", "Production Limit")]
        private SerializedDictionary<Tier, int> tier1TownLimits;

        [SerializeField, SerializedDictionary("Good Tier", "Production Limit")]
        private SerializedDictionary<Tier, int> tier2TownLimits;

        [SerializeField, SerializedDictionary("Good Tier", "Production Limit")]
        private SerializedDictionary<Tier, int> tier3TownLimits;

        [Header("Production Rates")]
        [SerializeField, SerializedDictionary("Good Tier", "Production Rate")]
        private SerializedDictionary<Tier, int> tier1TownProduction;

        [SerializeField, SerializedDictionary("Good Tier", "Production Rate")]
        private SerializedDictionary<Tier, int> tier2TownProduction;

        [SerializeField, SerializedDictionary("Good Tier", "Production Rate")]
        private SerializedDictionary<Tier, int> tier3TownProduction;

        [SerializeField, SerializedDictionary("Building Tier", "Cost")]
        private SerializedDictionary<Tier, int[]> upgradeCosts;

        public int? GetProductionRate(Tier townTier, Tier goodTier)
        {
            var limitDict = townTier switch
            {
                Tier.Tier1 => tier1TownProduction,
                Tier.Tier2 => tier2TownProduction,
                Tier.Tier3 => tier3TownProduction,
                _ => throw new ArgumentOutOfRangeException(nameof(townTier), townTier, null)
            };

            return limitDict.TryGetValue(goodTier, out var value) ? value : null;
        }

        public int? GetLimit(Tier townTier, Tier goodTier)
        {
            var limitDict = townTier switch
            {
                Tier.Tier1 => tier1TownLimits,
                Tier.Tier2 => tier2TownLimits,
                Tier.Tier3 => tier3TownLimits,
                _ => throw new ArgumentOutOfRangeException(nameof(townTier), townTier, null)
            };

            return limitDict.TryGetValue(goodTier, out var value) ? value : null;
        }

        public int? GetUpgradeCost(Tier tier, int buildingIndex)
        {
            if (buildingIndex > upgradeCosts[tier].Length) return null;
            return upgradeCosts[tier][buildingIndex];
        }
    }
}