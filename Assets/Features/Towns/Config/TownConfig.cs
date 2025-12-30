using System;
using AYellowpaper.SerializedCollections;
using Common.Types;
using Common.Utility;
using UnityEngine;

namespace Features.Towns.Config
{
    [CreateAssetMenu(
        fileName = nameof(TownConfig),
        menuName = AssetMenu.ConfigDataFolder + nameof(TownConfig))]
    public sealed class TownConfig : ScriptableObject
    {
        [field: SerializeField, SerializedDictionary("Town Tier", "Funds /day")]
        public SerializedDictionary<Tier, float> FundRate { get; private set; }

        [field: SerializeField, SerializedDictionary("Town Tier", "Inventory Slots")]
        public SerializedDictionary<Tier, int> InventorySlotsPerTier { get; private set; }

        [Header("Consumption Rates")]
        [SerializeField, SerializedDictionary("Good Tier", "Consumption /day")]
        private SerializedDictionary<Tier, float> tier1ConsumptionRate;

        [SerializeField, SerializedDictionary("Good Tier", "Consumption /day")]
        private SerializedDictionary<Tier, float> tier2ConsumptionRate;

        [SerializeField, SerializedDictionary("Good Tier", "Consumption /day")]
        private SerializedDictionary<Tier, float> tier3ConsumptionRate;

        [SerializeField]
        private int minStartFunds = 300, maxStartFunds = 700;

        [SerializeField]
        private int minStartGoods = 5, maxStartGoods = 25;

        public float? GetConsumptionRate(Tier townTier, Tier goodTier)
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

        public int GetStartFunds()
        {
            return UnityEngine.Random.Range(minStartFunds, maxStartFunds);
        }

        public int GetStartGoods()
        {
            return UnityEngine.Random.Range(minStartGoods, maxStartGoods);
        }
    }
}