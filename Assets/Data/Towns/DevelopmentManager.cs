using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Data.Configuration;
using UnityEngine;

namespace Data.Towns
{
    public sealed class DevelopmentManager
    {
        public IReadOnlyList<IGrowthModifier> GrowthModifiers => _growthModifiers;

        public event Action GrowthModifiersChanged;

        public Observable<float> DevelopmentScore { get; } = new();
        public Observable<float> DevelopmentTrend { get; } = new();
        public Observable<DevelopmentTrend> GrowthTrend { get; } = new();

        private readonly Town _town;
        private readonly TownDevelopmentConfig _townDevelopmentConfig;
        private readonly GoodsConfig _goodsConfig;
        private readonly List<IGrowthModifier> _growthModifiers;

        private DevelopmentTable _developmentTable;

        public DevelopmentManager(Town town)
        {
            _town = town;
            _growthModifiers = new();
            _townDevelopmentConfig = ConfigurationManager.Instance.TownDevelopmentConfig;
            _goodsConfig = ConfigurationManager.Instance.GoodsConfig;
            UpdateDevelopmentTable();
        }

        public void ComputeDevelopment()
        {
            _growthModifiers.Clear();

            ApplyProducerModifiers();
            ApplyForeignGoodsModifiers();
            ApplyModifiers();
        }

        private void ApplyProducerModifiers()
        {
            var producers = _town.Producer.GetProducerCount();

            foreach (var (tier, count) in producers)
            {
                var producerInfluence = _townDevelopmentConfig.ProducerGrowthInfluence.Get(_town.Tier.Value, tier);
                if (count <= 1)
                    continue;

                var modifierValue = (count - 1) * producerInfluence;
                var modifier = new ProducerModifier(modifierValue, count, tier);
                _growthModifiers.Add(modifier);
            }
        }

        private void ApplyForeignGoodsModifiers()
        {
            var foreignGoodCounts = _town.Inventory.Goods.Keys
                .Where(good => !_town.Producer.IsProduced(good))
                .GroupBy(good => _goodsConfig.ConfigData[good].Tier)
                .ToDictionary(grouping => grouping.Key, kvPair => kvPair.Count());

            foreach (var (tier, count) in foreignGoodCounts)
            {
                var modifierValue = _developmentTable.GetDevelopmentTrend(tier, count);
                _growthModifiers.Add(new ForeignGoodsModifier(modifierValue, count, tier));
            }
        }

        private void ApplyModifiers()
        {
            GrowthModifiersChanged?.Invoke();

            var developmentTrend = _growthModifiers.Sum(modifier => modifier.Value);
            // reset development trend to 0, if we are at 0 development, mostly for visual purposes
            if (DevelopmentScore <= 0 && developmentTrend < 0)
            {
                developmentTrend = 0;
            }

            var developmentScore = DevelopmentScore.Value + developmentTrend;

            // upgrade town if needed
            if (developmentScore >= 100 && _town.Tier.Value < Tier.Tier3)
            {
                _town.Upgrade();
                UpdateDevelopmentTable();
                developmentScore = 0;
            }

            developmentScore = Mathf.Clamp(developmentScore, 0, 100);

            DevelopmentTrend.Value = developmentTrend;
            DevelopmentScore.Value = developmentScore;

            UpdateGrowthTrend();
        }


        private void UpdateGrowthTrend()
        {
            var newGrowthTrend = _townDevelopmentConfig.GetTrend(DevelopmentTrend);
            if (GrowthTrend == newGrowthTrend) return;

            GrowthTrend.Value = newGrowthTrend;
        }

        private void UpdateDevelopmentTable()
        {
            _developmentTable = _townDevelopmentConfig.DevelopmentTables[_town.Tier];
        }
    }
}