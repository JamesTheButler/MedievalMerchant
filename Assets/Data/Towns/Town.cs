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
        // TODO: should come from config file
        private const int BaseFundsPerTick = 20;

        private readonly DevelopmentConfig _developmentConfig;
        private readonly Producer _producer;
        private readonly TierBasedInventoryPolicy _inventoryPolicy;
        private readonly GrowthTrendConfig _growthConfig;
        private readonly GoodsConfig _goodsConfig;

        public Inventory Inventory { get; }
        public string Name { get; }
        public Vector2Int GridLocation { get; }
        public Vector2 WorldLocation { get; }
        public HashSet<Good> AvailableGoods { get; }

        public Observable<Tier> Tier { get; } = new();
        public Observable<float> DevelopmentScore { get; } = new();
        public Observable<float> DevelopmentTrend { get; } = new();
        public Observable<GrowthTrend> GrowthTrend { get; } = new();

        public Producer Producer => _producer;

        private DevelopmentTable _developmentTable;

        public Town(TownSetupInfo setupInfo,
            Vector2Int gridLocation,
            Vector2 worldLocation,
            IEnumerable<Good> availableGoods)
        {
            _inventoryPolicy = new TierBasedInventoryPolicy();

            GridLocation = gridLocation;
            WorldLocation = worldLocation;
            _goodsConfig = ConfigurationManager.Instance.GoodsConfig;
            _developmentConfig = ConfigurationManager.Instance.DevelopmentConfig;
            _growthConfig = ConfigurationManager.Instance.GrowthTrendConfig;
            AvailableGoods = availableGoods.ToHashSet();

            Name = setupInfo.NameGenerator.GenerateName();

            Tier.Value = Data.Tier.Tier1;
            _inventoryPolicy.SetTier(Data.Tier.Tier1);
            UpdateDevelopmentTable();

            // initial funds and goods
            Inventory = new Inventory(_inventoryPolicy);
            _producer = new Producer(this);

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
            _producer.AddProducer(good, index);
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
            // update production multiplier
            var multiplier = DevelopmentScore.Value switch
            {
                < 20 => 0.5f,
                > 80 => 2f,
                _ => 1f,
            };
            _producer.SetProductionMultiplier(multiplier * (float)Tier.Value);
            _producer.Produce();

            // if development trend is positive, add funds
            var trendFundMultiplier = DevelopmentTrend > 0 ? DevelopmentTrend : 1f;
            Inventory.AddFunds((int)(BaseFundsPerTick * (int)Tier.Value * trendFundMultiplier));
        }

        private void Consume()
        {
            foreach (var good in Inventory.Goods.Keys.ToList())
            {
                Inventory.RemoveGood(good, 1);
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