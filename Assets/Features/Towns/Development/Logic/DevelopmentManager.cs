using System;
using System.Collections.Generic;
using System.Linq;
using Common.Infrastructure;
using Common.Infrastructure.Modifiable;
using Common.Infrastructure.Observation;
using Common.Types;
using Features.Goods.Config;
using Features.Towns.Development.Config;
using Features.Towns.Production.Logic;
using UnityEngine;

namespace Features.Towns.Development.Logic
{
    public sealed class DevelopmentManager
    {
        public Observable<Tier> Tier { get; } = new(Common.Types.Tier.Tier1);
        public ModifiableVariable DevelopmentTrend { get; }
        public IReadOnlyObservable<float> DevelopmentScore => _developmentScore;
        public Observable<DevelopmentTrend> GrowthTrend { get; } = new();

        private readonly Town _town;
        private readonly TownDevelopmentConfig _townDevelopmentConfig;
        private readonly GoodResources _goodResources;
        private readonly Observable<float> _developmentScore  = new();
        private readonly Dictionary<Tier, ProducerDevelopmentModifier> _producerModifiers = new();
        private readonly Dictionary<Tier, StoredGoodsDevelopmentModifier> _storedGoodsModifier = new();

        private bool _isDegrowthLocked;

        public DevelopmentManager(Town town)
        {
            _town = town;
            _townDevelopmentConfig = ConfigurationManager.Configurations.TownDevelopmentConfig;
            _goodResources = ResourceManager.Instance.GoodResources;

            var loc = ResourceManager.Instance.LocalizationResources.Town;
            var modifierTitle = loc.DevTrendModifierTitle.GetLocalizedString();
            DevelopmentTrend =  new ModifiableVariable(modifierTitle, true);
            
            _town.ProductionManager.ProductionAdded.Observe(OnProducerAdded);
            _town.Inventory.GoodUpdated += OnGoodAdded;

            DevelopmentTrend.AddModifier(new BaseDegrowthModifier(Tier));
        }

        ~DevelopmentManager()
        {
            _town.ProductionManager.ProductionAdded.StopObserving(OnProducerAdded);
            _town.Inventory.GoodUpdated -= OnGoodAdded;
        }

        public void AddDevelopmentChange(float developmentChange)
        {
            if (_isDegrowthLocked && developmentChange < 0f)
                return;

            var developmentScore = _developmentScore + developmentChange;
            developmentScore = Mathf.Clamp(developmentScore, 0, 100);
            _developmentScore.Value = developmentScore;
            UpdateGrowthTrend();
        }

        public void Upgrade()
        {
            var oldTier = Tier.Value;
            var newTier = (Tier)Math.Min((int)Tier.Value + 1, (int)Common.Types.Tier.Tier3);

            if (oldTier == newTier)
                return;

            Tier.Value = newTier;
            _developmentScore.Value = 0;
            Debug.Log($"{_town.Name} upgraded to {Tier}");
        }

        public void LockDegrowth(bool isLocked)
        {
            _isDegrowthLocked = isLocked;
        }

        private void OnProducerAdded(Producer producer)
        {
            var goodTier = _goodResources.ResourceData[producer.ProducedGood].Tier;
            RefreshProducerModifiers(goodTier);
        }

        private void OnGoodAdded(Good addedGood, int _)
        {
            // early out, as we only care about non-produced goods
            if (_town.ProductionManager.IsProduced(addedGood))
                return;

            var goodTier = _goodResources.ResourceData[addedGood].Tier;
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
            var modifier = new ProducerDevelopmentModifier(modifierValue, newProducerCount, goodTier);
            DevelopmentTrend.AddModifier(modifier);
            _producerModifiers[goodTier] = modifier;
        }

        private void RefreshGoodsInInventoryModifiers(Tier goodTier)
        {
            var newCount = _town.Inventory.Goods.Keys
                .Count(good =>
                    !_town.ProductionManager.IsProduced(good) && _goodResources.ResourceData[good].Tier == goodTier);

            // modifier would not change
            if (_storedGoodsModifier.TryGetValue(goodTier, out var oldModifier) &&
                oldModifier.GoodCount == newCount)
                return;

            // TODO - STYLE: should use observable modifier
            DevelopmentTrend.RemoveModifier(oldModifier);

            var modifierValue = _townDevelopmentConfig.SoldGoodsGrowthInfluence.Get(Tier, goodTier) * newCount;
            var modifier = new StoredGoodsDevelopmentModifier(modifierValue, newCount, goodTier);
            DevelopmentTrend.AddModifier(modifier);
            _storedGoodsModifier[goodTier] = modifier;
        }

        private void UpdateGrowthTrend()
        {
            var newGrowthTrend = _townDevelopmentConfig.GetTrend(DevelopmentTrend);
            if (GrowthTrend == newGrowthTrend)
                return;

            GrowthTrend.Value = newGrowthTrend;
        }
    }
}