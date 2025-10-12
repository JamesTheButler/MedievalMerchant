using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Modifiable;
using Common.Types;
using Features.Goods.Config;
using Features.Towns.Development.Config;
using Features.Towns.Production.Logic;
using UnityEngine;

namespace Features.Towns.Development.Logic
{
    public sealed class DevelopmentManager
    {
        public ModifiableVariable DevelopmentTrend { get; } = new();
        public Observable<float> DevelopmentScore { get; } = new();
        public Observable<DevelopmentTrend> GrowthTrend { get; } = new();

        private readonly Town _town;
        private readonly TownDevelopmentConfig _townDevelopmentConfig;
        private readonly GoodsConfig _goodsConfig;

        private TownDevelopmentTable _townDevelopmentTable;

        private readonly Dictionary<Tier, ProducerModifier> _producerModifiers = new();
        private readonly Dictionary<Tier, GoodsInInventoryDevelopmentModifier> _goodsInInventoryModifier = new();

        public DevelopmentManager(Town town)
        {
            _town = town;
            _townDevelopmentConfig = ConfigurationManager.Instance.TownDevelopmentConfig;
            _goodsConfig = ConfigurationManager.Instance.GoodsConfig;

            _town.ProductionManager.ProductionAdded += OnProducerAdded;
            _town.Inventory.GoodUpdated += OnGoodAdded;

            UpdateDevelopmentTable(_town.Tier);
        }

        ~DevelopmentManager()
        {
            _town.ProductionManager.ProductionAdded -= OnProducerAdded;
            _town.Inventory.GoodUpdated -= OnGoodAdded;
        }

        private void OnProducerAdded(Producer producer)
        {
            var goodTier = _goodsConfig.ConfigData[producer.ProducedGood].Tier;
            RefreshProducerModifiers(goodTier);
        }

        private void OnGoodAdded(Good addedGood, int _)
        {
            // early out, as we only care about non-produced goods
            if (_town.ProductionManager.IsProduced(addedGood)) return;

            var goodTier = _goodsConfig.ConfigData[addedGood].Tier;
            RefreshGoodsInInventoryModifiers(goodTier);
        }

        private void RefreshProducerModifiers(Tier goodTier)
        {
            var newProducerCount = _town.ProductionManager.GetProducerCount(goodTier);
            var producerInfluence = _townDevelopmentConfig.ProducerGrowthInfluence.Get(_town.Tier.Value, goodTier);
            if (newProducerCount <= 1)
                return;

            // modifier would not change
            if (_producerModifiers.TryGetValue(goodTier, out var oldModifier) &&
                oldModifier.ProducerCount == newProducerCount)
                return;

            DevelopmentTrend.RemoveModifier(oldModifier);

            var modifierValue = (newProducerCount - 1) * producerInfluence;
            var modifier = new ProducerModifier(modifierValue, newProducerCount, goodTier);
            DevelopmentTrend.AddModifier(modifier);
            _producerModifiers[goodTier] = modifier;
        }

        private void RefreshGoodsInInventoryModifiers(Tier goodTier)
        {
            var newCount = _town.Inventory.Goods.Keys
                .Count(good =>
                    !_town.ProductionManager.IsProduced(good) && _goodsConfig.ConfigData[good].Tier == goodTier);

            // modifier would not change
            if (_goodsInInventoryModifier.TryGetValue(goodTier, out var oldModifier) &&
                oldModifier.GoodCount == newCount)
                return;

            DevelopmentTrend.RemoveModifier(oldModifier);

            var modifierValue = _townDevelopmentTable.GetDevelopmentTrend(goodTier, newCount);
            var modifier = new GoodsInInventoryDevelopmentModifier(modifierValue, newCount, goodTier);
            DevelopmentTrend.AddModifier(modifier);
            _goodsInInventoryModifier[goodTier] = modifier;
        }

        public void UpdateDevelopment()
        {
            var developmentScore = DevelopmentScore.Value + DevelopmentTrend.Value;

            // upgrade town if needed
            if (developmentScore >= 100 && _town.Tier.Value < Tier.Tier3)
            {
                DevelopmentScore.Value = 100; // make sure to update observers
            }

            developmentScore = Mathf.Clamp(developmentScore, 0, 100);

            DevelopmentScore.Value = developmentScore;

            UpdateGrowthTrend();
        }

        private void UpdateGrowthTrend()
        {
            var newGrowthTrend = _townDevelopmentConfig.GetTrend(DevelopmentTrend);
            if (GrowthTrend == newGrowthTrend) return;

            GrowthTrend.Value = newGrowthTrend;
        }

        private void UpdateDevelopmentTable(Tier tier)
        {
            _townDevelopmentTable = _townDevelopmentConfig.DevelopmentTables[tier];

            RefreshGoodsInInventoryModifiers(tier);
        }
    }
}