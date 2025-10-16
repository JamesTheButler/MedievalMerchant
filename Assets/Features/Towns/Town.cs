using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Types;
using Features.Goods.Config;
using Features.Inventory;
using Features.Towns.Config;
using Features.Towns.Development.Logic;
using Features.Towns.Development.Logic.Upgrades;
using Features.Towns.Flags;
using Features.Towns.Flags.Logic;
using Features.Towns.Production.Logic;
using UnityEngine;

namespace Features.Towns
{
    public sealed class Town
    {
        private readonly TierBasedInventoryPolicy _inventoryPolicy;
        private readonly TownConfig _townConfig;
        private readonly GoodsConfig _goodsConfig;

        private readonly Observable<float> _reputation = new();

        public Observable<Tier> Tier { get; } = new();

        public IReadOnlyObservable<float> Reputation => _reputation;

        public ProductionManager ProductionManager { get; }
        public DevelopmentManager DevelopmentManager { get; }
        public TownUpgradeManager UpgradeManager { get; }

        public Inventory.Inventory Inventory { get; }

        public string Name { get; }
        public FlagInfo FlagInfo { get; private set; }
        public Vector2Int GridLocation { get; }
        public Vector2 WorldLocation { get; }
        public HashSet<Good> AvailableGoods { get; }
        public Regions MainRegion { get; }
        public Regions Regions { get; }

        public Town(
            Vector2Int gridLocation,
            Vector2 worldLocation,
            Regions regions,
            IEnumerable<Good> availableGoods,
            FlagFactory flagFactory)
        {
            // TODO - BUG: this is not limiting slot amount
            _inventoryPolicy = new TierBasedInventoryPolicy();

            GridLocation = gridLocation;
            WorldLocation = worldLocation;
            Regions = regions;
            MainRegion = regions.GetRandom();

            _townConfig = ConfigurationManager.Instance.TownConfig;
            _goodsConfig = ConfigurationManager.Instance.GoodsConfig;
            AvailableGoods = availableGoods.ToHashSet();

            Name = _townConfig.GetNameGenerator(MainRegion).GenerateName();

            Tier.Value = Common.Types.Tier.Tier1;
            _inventoryPolicy.SetTier(Common.Types.Tier.Tier1);

            // initial funds and goods
            Inventory = new Inventory.Inventory(_inventoryPolicy);
            ProductionManager = new ProductionManager(this);
            DevelopmentManager = new DevelopmentManager(this);
            UpgradeManager = new TownUpgradeManager(this);

            Inventory.AddFunds(_townConfig.GetStartFunds());

            var startGood = AvailableGoods.GetRandom();
            AddProduction(startGood, 0);
            FlagInfo = flagFactory.CreateFlagInfo(MainRegion);
        }

        public void Tick()
        {
            Produce();
            DevelopmentManager.UpdateDevelopment();
            Consume();
        }

        public void AddProduction(Good good, int index)
        {
            ProductionManager.AddProducer(good, index);
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
            // goods production
            var townTier = Tier.Value;
            ProductionManager.Produce();

            // funds production
            var modifierMultiplier = 1 + UpgradeManager.FundsModifiers;
            var baseFundsPerTick = _townConfig.FundRate[townTier];
            var fundChange = baseFundsPerTick * modifierMultiplier;
            Inventory.AddFunds(fundChange);
        }

        private void Consume()
        {
            var townTier = Tier.Value;
            foreach (var good in Inventory.Goods.Keys.ToList())
            {
                // don't consume goods that are produced
                if (ProductionManager.IsProduced(good)) continue;

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

        public void AddReputation(float added)
        {
            // TODO - 0.4: apply modifiers
            // TODO - 0.4: set limit
            _reputation.Value += added;
        }

        public void RemoveReputation(float removed)
        {
            // TODO - 0.4: apply modifiers
            // TODO - 0.4: set limit
            _reputation.Value -= removed;
        }

        private void IncreaseTier()
        {
            Tier.Value = (Tier)Math.Min((int)Tier.Value + 1, (int)Common.Types.Tier.Tier3);
        }
    }
}