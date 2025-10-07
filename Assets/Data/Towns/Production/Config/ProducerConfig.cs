using System;
using AYellowpaper.SerializedCollections;
using Data.Configuration;
using UnityEngine;

namespace Data.Towns.Production.Config
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

        [Header("Production Rate")]
        [SerializeField, SerializedDictionary("Good Tier", "Production Rate")]
        private SerializedDictionary<Tier, float> baseProductionRates;

        [Header("Construction Costs")]
        [SerializeField, SerializedDictionary("Producer Tier", "Cost")]
        private SerializedDictionary<Tier, int[]> upgradeCosts;

        [field: SerializeField]
        public float ConsumptionRate { get; private set; } = 1f;

        public float GetProductionRate(Tier goodTier)
        {
            return baseProductionRates[goodTier];
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