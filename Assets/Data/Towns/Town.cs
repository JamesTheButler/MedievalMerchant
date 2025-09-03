using System;
using System.Collections.Generic;
using System.Linq;
using Data.Configuration;
using Data.Setup;
using UnityEngine;

namespace Data.Towns
{
    public sealed class Town
    {
        private readonly DevelopmentConfig _developmentConfig;
        private readonly Producer _producer;

        private const int StartGoodMultiplier = 25;
        private const int BaseFundsPerTick = 20;

        public event Action<Tier> TierChanged;
        public event Action<float> DevelopmentScoreChanged;
        public event Action<float> DevelopmentTrendChanged;

        public Inventory Inventory { get; } = new(new UnlimitedInventoryPolicy());
        public string Name { get; }
        public Tier Tier { get; private set; }
        public Vector2Int Location { get; }

        public float DevelopmentScore { get; private set; }
        public float DevelopmentTrend { get; private set; }

        public IEnumerable<Good> Production => _producer.ProducedGoods;

        private DevelopmentTable _developmentTable;

        public Town(TownSetupInfo setupInfo, Vector2Int location)
        {
            Location = location;
            _developmentConfig = ConfigurationManager.Instance.DevelopmentConfig;
            _producer = new Producer(setupInfo.Production);

            Name = setupInfo.NameGenerator.GenerateName();
            Tier = Tier.Tier1;
            UpdateDevelopmentTable();

            // initial funds and goods
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
            var oldTier = Tier;
            IncreaseTier();

            Debug.Log($"{Name} upgraded to {Tier}");
            _producer.UpgradeTier(Tier);

            if (oldTier != Tier)
            {
                _developmentTable = _developmentConfig.DevelopmentTables[Tier];
                TierChanged?.Invoke(Tier);
            }
        }

        private void Produce()
        {
            // update production multiplier
            var multiplier = DevelopmentScore switch
            {
                < 20 => 0.5f,
                > 80 => 2f,
                _ => 1f,
            };
            _producer.SetProductionMultiplier(multiplier);

            // if development trend is positive, add funds
            var trendFundMultiplier = DevelopmentTrend > 0 ? DevelopmentTrend : 1f;
            Inventory.AddFunds((int)(BaseFundsPerTick * (int)Tier * trendFundMultiplier));

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
            DevelopmentTrend = _developmentTable.GetDevelopmentTrend(
                goodsPerTier[Tier.Tier1],
                goodsPerTier[Tier.Tier2],
                goodsPerTier[Tier.Tier3]) * _developmentConfig.DevelopmentMultiplier;

            if (DevelopmentScore <= 0 && DevelopmentTrend < 0)
            {
                DevelopmentTrend = 0;
            }

            DevelopmentScore += DevelopmentTrend;
            if (DevelopmentScore >= 100 && Tier <= Tier.Tier2)
            {
                Upgrade();
                DevelopmentScore = 0;
            }

            DevelopmentScore = Mathf.Clamp(DevelopmentScore, 0, 100);
            DevelopmentTrendChanged?.Invoke(DevelopmentTrend);
            DevelopmentScoreChanged?.Invoke(DevelopmentScore);
        }

        private void UpdateDevelopmentTable()
        {
            _developmentTable = _developmentConfig.DevelopmentTables[Tier];
        }

        private void IncreaseTier()
        {
            Tier = (Tier)Math.Min((int)Tier + 1, (int)Tier.Tier3);
        }
    }
}