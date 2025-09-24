using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Data.Configuration;
using UnityEngine;

namespace Data.Towns
{
    public sealed class Producer
    {
        public event Action<Good> ProductionAdded;

        private readonly ProducerConfig _producerConfig;
        private readonly Town _town;

        private readonly Dictionary<Tier, Good?[]> _producers;

        private float _multiplier = 1f;

        public Producer(Town town)
        {
            _town = town;
            _producerConfig = ConfigurationManager.Instance.ProducerConfig;
            _producers = new Dictionary<Tier, Good?[]>
            {
                { Tier.Tier1, new Good?[] { null, null, null } },
                { Tier.Tier2, new Good?[] { null, null, null } },
                { Tier.Tier3, new Good?[] { null, null, null } },
            };
        }

        public IEnumerable<Good> AllProducers => _producers[Tier.Tier1]
            .Concat(_producers[Tier.Tier2])
            .Concat(_producers[Tier.Tier3])
            .WhereNotNull();


        public bool IsProduced(Good good)
        {
            return _producers.Any(producer => producer.Value.Contains(good));
        }

        private readonly GoodsConfig _goodsConfig = ConfigurationManager.Instance.GoodsConfig;

        public Good?[] GetProducers(Tier tier)
        {
            return _producers[tier];
        }

        public int GetIndexOfProducedGood(Good good)
        {
            var tier = _goodsConfig.ConfigData[good].Tier;
            return GetProducers(tier).ToList().IndexOf(good);
        }

        public bool CanAddProducer(Good good, int index)
        {
            var tier = _goodsConfig.ConfigData[good].Tier;
            return GetProducers(tier)[index] == null;
        }

        public void AddProducer(Good good, int index)
        {
            if (!CanAddProducer(good, index)) return;

            var tier = _goodsConfig.ConfigData[good].Tier;
            var productions = GetProducers(tier);
            productions[index] = good;
            ProductionAdded?.Invoke(good);
        }

        public void Produce()
        {
            foreach (var (good, producedAmount) in GetProductions())
            {
                var limit = GetProductionLimit(_town.Tier, good);
                var currentInventoryAmount = _town.Inventory.Goods.GetValueOrDefault(good, 0);
                var cappedAmount = Mathf.Min(producedAmount, Mathf.Max(0, limit - currentInventoryAmount));
                _town.Inventory.AddGood(good, cappedAmount);
            }
        }

        public void SetProductionMultiplier(float multiplier)
        {
            _multiplier = multiplier;
        }

        private int GetProductionLimit(Tier townTier, Good good)
        {
            var goodTier = _goodsConfig.ConfigData[good].Tier;
            var limit = _producerConfig.GetLimit(townTier, goodTier);
            if (limit != null)
                return limit.Value;

            Debug.LogError($"No production limit is set for town {townTier} and good {goodTier}.");
            return 0;
        }

        private int GetProduction(Tier townTier, Good good)
        {
            var goodTier = _goodsConfig.ConfigData[good].Tier;
            var productionRate = _producerConfig.GetProductionRate(townTier, goodTier);
            if (productionRate != null)
                return Mathf.RoundToInt(productionRate.Value * _multiplier);

            Debug.LogError($"No production rate is set for town {townTier} and good {goodTier}.");
            return 0;
        }

        private IReadOnlyDictionary<Good, int> GetProductions()
        {
            return GetProduction(Tier.Tier1)
                .Concat(GetProduction(Tier.Tier2))
                .Concat(GetProduction(Tier.Tier3))
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        private IReadOnlyDictionary<Good, int> GetProduction(Tier tier)
        {
            var producers = _producers[tier];
            return producers.WhereNotNull().ToDictionary(good => good, good => GetProduction(tier, good));
        }
    }
}