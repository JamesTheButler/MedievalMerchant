using System;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Data.Configuration
{
    [CreateAssetMenu(
        fileName = nameof(TownConfig),
        menuName = AssetMenu.ConfigDataFolder + nameof(TownConfig))]
    public sealed class TownConfig : ScriptableObject
    {
        [field: SerializeField, SerializedDictionary("Town Tier", "Funds Per Tick")]
        public SerializedDictionary<Tier, float> FundRate { get; private set; }

        [Header("Consumption Rates")]
        [SerializeField, SerializedDictionary("Good Tier", "Consumption Rate")]
        private SerializedDictionary<Tier, int> tier1ConsumptionRate;

        [SerializeField, SerializedDictionary("Good Tier", "Consumption Rate")]
        private SerializedDictionary<Tier, int> tier2ConsumptionRate;

        [SerializeField, SerializedDictionary("Good Tier", "Consumption Rate")]
        private SerializedDictionary<Tier, int> tier3ConsumptionRate;

        public int? GetConsumptionRate(Tier townTier, Tier goodTier)
        {
            var limitDict = townTier switch
            {
                Tier.Tier1 => tier1ConsumptionRate,
                Tier.Tier2 => tier2ConsumptionRate,
                Tier.Tier3 => tier3ConsumptionRate,
                _ => throw new ArgumentOutOfRangeException(nameof(townTier), townTier, null)
            };

            return limitDict.TryGetValue(goodTier, out var value) ? value : null;
        }
    }
}