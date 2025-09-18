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
        private readonly DevelopmentConfig _developmentConfig;
        private readonly TierBasedInventoryPolicy _inventoryPolicy;
        private readonly GrowthTrendConfig _growthConfig;
        private readonly TownConfig _townConfig;
        private readonly GoodsConfig _goodsConfig;

        public Observable<Tier> Tier { get; } = new();
        public Observable<float> DevelopmentScore { get; } = new();
        public Observable<float> DevelopmentTrend { get; } = new();
        public Observable<GrowthTrend> GrowthTrend { get; } = new();

        public Inventory Inventory { get; }
        public string Name { get; }
        public Vector2Int GridLocation { get; }
        public Vector2 WorldLocation { get; }
        public HashSet<Good> AvailableGoods { get; }
        public Producer Producer { get; }

        private DevelopmentTable _developmentTable;

        public Town(TownSetupInfo setupInfo,
            Vector2Int gridLocation,
            Vector2 worldLocation,
            IEnumerable<Good> availableGoods)
        {
            _inventoryPolicy = new TierBasedInventoryPolicy();

            GridLocation = gridLocation;
            WorldLocation = worldLocation;
            _developmentConfig = ConfigurationManager.Instance.DevelopmentConfig;
            _growthConfig = ConfigurationManager.Instance.GrowthTrendConfig;
            _townConfig = ConfigurationManager.Instance.TownConfig;
            _goodsConfig = ConfigurationManager.Instance.GoodsConfig;
            AvailableGoods = availableGoods.ToHashSet();

            Name = setupInfo.NameGenerator.GenerateName();

            Tier.Value = Data.Tier.Tier1;
            _inventoryPolicy.SetTier(Data.Tier.Tier1);
            UpdateDevelopmentTable();

            // initial funds and goods
            Inventory = new Inventory(_inventoryPolicy);
            Producer = new Producer(this);

            Inventory.AddFunds(setupInfo.InitialFunds);

            AddProduction(AvailableGoods.GetRandom(), 0);
        }

        public void Tick()
        {
            Produce();
            Develop();
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
                UpdateDevelopmentTable();
                _inventoryPolicy.SetTier(Tier);
            }
        }

        private void Produce()
        {
            var townTier = Tier.Value;
            // update production multiplier
            var multiplier = DevelopmentScore.Value switch
            {
                < 20 => 0.5f,
                > 80 => 2f,
                _ => 1f,
            };
            Producer.SetProductionMultiplier(multiplier);
            Producer.Produce();

            // if development trend is positive, add funds
            var trendFundMultiplier = DevelopmentTrend > 0 ? DevelopmentTrend : 1f;
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

        private void Develop()
        {
            var goodsPerTier = Inventory.GoodsPerTier();

            var newDevTrend = _developmentTable.GetDevelopmentTrend(
                goodsPerTier[Data.Tier.Tier1],
                goodsPerTier[Data.Tier.Tier2],
                goodsPerTier[Data.Tier.Tier3]) * _developmentConfig.DevelopmentMultiplier;

            if (DevelopmentScore <= 0 && newDevTrend < 0)
            {
                newDevTrend = 0;
            }

            var newDevScore = DevelopmentScore.Value + newDevTrend;

            if (newDevScore >= 100 && Tier <= Data.Tier.Tier2)
            {
                Upgrade();
                newDevScore = 0;
            }

            newDevScore = Mathf.Clamp(newDevScore, 0, 100);

            DevelopmentTrend.Value = newDevTrend;
            DevelopmentScore.Value = newDevScore;

            UpdateGrowthTrend();
        }

        private void UpdateGrowthTrend()
        {
            var newGrowthTrend = _growthConfig.GetTrend(DevelopmentTrend);
            if (GrowthTrend == newGrowthTrend) return;

            GrowthTrend.Value = newGrowthTrend;
        }

        private void UpdateDevelopmentTable()
        {
            _developmentTable = _developmentConfig.DevelopmentTables[Tier];
        }

        private void IncreaseTier()
        {
            Tier.Value = (Tier)Math.Min((int)Tier.Value + 1, (int)Data.Tier.Tier3);
        }
    }
}