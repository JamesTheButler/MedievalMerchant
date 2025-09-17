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

        // TODO: config file
        private const int BaseProduction = 4;

        // TODO: config file
        private readonly Dictionary<Tier, int> _productionLimits = new()
        {
            { Tier.Tier1, 100 },
            { Tier.Tier2, 150 },
            { Tier.Tier3, 200 },
        };

        private readonly Good?[] _tier1Producers = new Good?[3];
        private readonly Good?[] _tier2Producers = new Good?[3];
        private readonly Good?[] _tier3Producers = new Good?[3];

        private float _multiplier = 1f;

        private readonly Town _town;

        public Producer(Town town)
        {
            _town = town;
        }

        public IEnumerable<Good> AllProducers => _tier1Producers
            .Concat(_tier2Producers)
            .Concat(_tier3Producers)
            .WhereNotNull();


        public bool IsProduced(Good good)
        {
            return _tier1Producers.Contains(good) ||
                   _tier2Producers.Contains(good) ||
                   _tier3Producers.Contains(good);
        }

        private readonly GoodsConfig _goodsConfig = ConfigurationManager.Instance.GoodsConfig;

        public Good?[] GetProducers(Tier tier)
        {
            return tier switch
            {
                Tier.Tier1 => _tier1Producers,
                Tier.Tier2 => _tier2Producers,
                Tier.Tier3 => _tier3Producers,
                _ => _tier1Producers
            };
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

        // TODO: should come from config file
        private int GetProductionLimit(Tier townTier, Good good)
        {
            // TODO: should have different limits based on town and good tier
            //  T1 town: 50xT1
            //  T2 town: 100xT1, 25xT2
            //  T3 town: 150xT1, 50xT2, 25xT3
            return _productionLimits[townTier];
        }

        // TODO: should come from config file
        private int GetProduction(Tier townTier, Good good)
        {
            // TODO: should have different productions based on town and good tier
            return (int)(BaseProduction * _multiplier);
        }

        private IReadOnlyDictionary<Good, int> GetProductions()
        {
            return GetProduction(_tier1Producers, Tier.Tier1)
                .Concat(GetProduction(_tier2Producers, Tier.Tier2))
                .Concat(GetProduction(_tier3Producers, Tier.Tier3))
                .ToDictionary(x => x.Key, x => x.Value);
        }

        private IReadOnlyDictionary<Good, int> GetProduction(Good?[] producers, Tier tier)
        {
            return producers.WhereNotNull().ToDictionary(good => good, good => GetProduction(tier, good));
        }
    }
}