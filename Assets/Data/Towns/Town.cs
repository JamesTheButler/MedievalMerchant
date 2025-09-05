using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Data.Configuration;
using Data.Setup;
using Data.Trade;
using UnityEngine;

namespace Data.Towns
{
    public sealed class Town
    {
        private readonly DevelopmentConfig _developmentConfig;
        private readonly Producer _producer;
        private readonly TierBasedInventoryPolicy _inventoryPolicy;
        private readonly GrowthTrendConfig _growthConfig;

        private const int StartGoodMultiplier = 25;
        private const int BaseFundsPerTick = 20;

        public Inventory Inventory { get; }
        public string Name { get; }
        public Vector2Int GridLocation { get; }
        public Vector2 WorldLocation { get; }

        public Observable<Tier> Tier { get; } = new();
        public Observable<float> DevelopmentScore { get;  } = new();
        public Observable<float> DevelopmentTrend { get;  } = new();
        public Observable<GrowthTrend> GrowthTrend { get; } = new();

        public IEnumerable<Good> Production => _producer.ProducedGoods;

        private DevelopmentTable _developmentTable;

        public Town(TownSetupInfo setupInfo, Vector2Int gridLocation, Vector2 worldLocation)
        {
            _inventoryPolicy = new TierBasedInventoryPolicy();

            GridLocation = gridLocation;
            WorldLocation = worldLocation;
            _developmentConfig = ConfigurationManager.Instance.DevelopmentConfig;
            _growthConfig = ConfigurationManager.Instance.GrowthTrendConfig;
            _producer = new Producer(setupInfo.Production);

            Name = setupInfo.NameGenerator.GenerateName();

            Tier.Value = Data.Tier.Tier1;
            _inventoryPolicy.SetTier(Data.Tier.Tier1);
            UpdateDevelopmentTable();

            // initial funds and goods
            Inventory = new Inventory(_inventoryPolicy);
            Inventory.AddFunds(setupInfo.InitialFunds);
            foreach (var (good, amount) in _producer.Produce())
            {
                Inventory.AddGood(good, amount * StartGoodMultiplier);
            }
        }

        public void Tick()
        {
            Produce();
            Develop();
            Consume();
        }

        public void Upgrade()
        {
            var oldTier = Tier.Value;
            IncreaseTier();

            Debug.Log($"{Name} upgraded to {Tier}");
            _producer.UpgradeTier(Tier);

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
            _producer.SetProductionMultiplier(multiplier);

            // if development trend is positive, add funds
            var trendFundMultiplier = DevelopmentTrend > 0 ? DevelopmentTrend : 1f;
            Inventory.AddFunds((int)(BaseFundsPerTick * (int)Tier.Value * trendFundMultiplier));

            var production = _producer.Produce();
            foreach (var (good, amount) in production)
            {
                Inventory.AddGood(good, amount);
            }
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