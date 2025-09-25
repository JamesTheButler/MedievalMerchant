using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Data.Configuration;
using Data.Trade;
using UnityEngine;

namespace Data.Towns
{
    public sealed class Town
    {
        private readonly TownDevelopmentConfig _townDevelopmentConfig;
        private readonly TierBasedInventoryPolicy _inventoryPolicy;
        private readonly TownDevelopmentConfig _growthConfig;
        private readonly TownConfig _townConfig;
        private readonly GoodsConfig _goodsConfig;

        public Observable<Tier> Tier { get; } = new();

        public Inventory Inventory { get; }
        public string Name { get; }
        public Vector2Int GridLocation { get; }
        public Vector2 WorldLocation { get; }
        public HashSet<Good> AvailableGoods { get; }
        public Producer Producer { get; }
        public DevelopmentManager DevelopmentManager { get; }
        public Regions Regions { get; }

        public Town(TownSetupInfo setupInfo,
            Vector2Int gridLocation,
            Vector2 worldLocation,
            Regions regions,
            IEnumerable<Good> availableGoods)
        {
            _inventoryPolicy = new TierBasedInventoryPolicy();

            GridLocation = gridLocation;
            WorldLocation = worldLocation;
            Regions = regions;

            _townDevelopmentConfig = ConfigurationManager.Instance.TownDevelopmentConfig;
            _growthConfig = ConfigurationManager.Instance.TownDevelopmentConfig;
            _townConfig = ConfigurationManager.Instance.TownConfig;
            _goodsConfig = ConfigurationManager.Instance.GoodsConfig;
            AvailableGoods = availableGoods.ToHashSet();

            Name = setupInfo.NameGenerator.GenerateName();

            Tier.Value = Data.Tier.Tier1;
            _inventoryPolicy.SetTier(Data.Tier.Tier1);

            // initial funds and goods
            Inventory = new Inventory(_inventoryPolicy);
            Producer = new Producer(this);
            DevelopmentManager = new DevelopmentManager(this);

            Inventory.AddFunds(setupInfo.InitialFunds);

            AddProduction(AvailableGoods.GetRandom(), 0);
        }

        public void Tick()
        {
            Produce();
            DevelopmentManager.ComputeDevelopment();
            Consume();
        }

        public void AddProduction(Good good, int index)
        {
            Producer.AddProducer(good, index);
        }

        public void Upgrade()
        {
            var oldTier = Tier.Value;
            IncreaseTier();

            Debug.Log($"{Name} upgraded to {Tier}");

            if (oldTier != Tier)
            {
                _inventoryPolicy.SetTier(Tier);
            }
        }

        private void Produce()
        {
            var townTier = Tier.Value;
            // update production multiplier
            var multiplier = DevelopmentManager.DevelopmentScore.Value switch
            {
                < 20 => 0.5f,
                > 80 => 2f,
                _ => 1f,
            };
            Producer.SetProductionMultiplier(multiplier);
            Producer.Produce();

            // if development trend is positive, add funds
            var trendFundMultiplier =
                DevelopmentManager.DevelopmentTrend > 0 ? DevelopmentManager.DevelopmentTrend : 1f;
            var fundsPerTick = _townConfig.FundRate[townTier];
            Inventory.AddFunds(Mathf.RoundToInt(fundsPerTick * trendFundMultiplier));
        }

        private void Consume()
        {
            var townTier = Tier.Value;
            foreach (var good in Inventory.Goods.Keys.ToList())
            {
                // don't consume goods that are produced
                if (Producer.IsProduced(good)) continue;

                var goodTier = _goodsConfig.ConfigData[good].Tier;
                var consumptionRate = _townConfig.GetConsumptionRate(townTier, goodTier);

                if (consumptionRate == null)
                {
                    Debug.LogError($"No consumption rate is set for town {townTier} and good {goodTier}.");
                    continue;
                }

                Inventory.RemoveGood(good, consumptionRate.Value);
            }
        }

        private void IncreaseTier()
        {
            Tier.Value = (Tier)Math.Min((int)Tier.Value + 1, (int)Data.Tier.Tier3);
        }
    }
}