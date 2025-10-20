using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Modifiable;
using Common.Types;
using Features.Goods.Config;
using Features.Inventory;
using Features.Towns.Config;
using Features.Towns.Development.Logic;
using Features.Towns.Development.Logic.Milestones;
using Features.Towns.Flags;
using Features.Towns.Flags.Logic;
using Features.Towns.Production.Logic;
using UnityEngine;

namespace Features.Towns
{
    public sealed class Town
    {
        private const int DefaultInventorySlots = 3;

        private readonly SlotBasedInventoryPolicy _inventoryPolicy;
        private readonly TownConfig _townConfig;
        private readonly GoodsConfig _goodsConfig;

        private readonly Observable<float> _reputation = new();

        public Observable<Tier> Tier { get; } = new();

        public IReadOnlyObservable<float> Reputation => _reputation;

        public ProductionManager ProductionManager { get; }
        public DevelopmentManager DevelopmentManager { get; }
        public TownUpgradeManager UpgradeManager { get; }
        public Inventory.Inventory Inventory { get; }
        public ModifiableVariable FundsChange { get; }

        public string Name { get; }
        public FlagInfo FlagInfo { get; private set; }
        public Vector2Int GridLocation { get; }
        public Vector2 WorldLocation { get; }
        public HashSet<Good> AvailableGoods { get; }
        public Region MainRegion { get; }
        public Regions Regions { get; }

        public Town(
            Vector2Int gridLocation,
            Vector2 worldLocation,
            Regions regions,
            IEnumerable<Good> availableGoods,
            FlagFactory flagFactory)
        {
            _inventoryPolicy = new SlotBasedInventoryPolicy();

            GridLocation = gridLocation;
            WorldLocation = worldLocation;
            Regions = regions;
            MainRegion = regions.GetRandom();

            _townConfig = ConfigurationManager.Instance.TownConfig;
            _goodsConfig = ConfigurationManager.Instance.GoodsConfig;
            AvailableGoods = availableGoods.ToHashSet();

            Name = _townConfig.NameGenerators[MainRegion].GenerateName();

            Tier.Value = Common.Types.Tier.Tier1;
            _inventoryPolicy.AddSlots(Tier, DefaultInventorySlots);

            // initial funds and goods
            Inventory = new Inventory.Inventory(_inventoryPolicy);
            ProductionManager = new ProductionManager(this);
            ProductionManager.ProductionAdded += OnProductionManagerOnProductionAdded;
            DevelopmentManager = new DevelopmentManager(this);
            UpgradeManager = new TownUpgradeManager(this);
            UpgradeManager.MilestoneModifierAdded += OnMilestoneModifierAdded;
            UpgradeManager.MilestoneModifierRemoved += OnMilestoneModifierRemoved;

            Inventory.AddFunds(_townConfig.GetStartFunds());
            var baseModifier = new BaseTownFundsProduction(_townConfig.FundRate[Tier], Tier);
            FundsChange = new ModifiableVariable("Funds change per day", baseModifier, true);

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
                _inventoryPolicy.AddSlots(Tier, DefaultInventorySlots);
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

        private void OnProductionManagerOnProductionAdded(Producer producer)
        {
            _inventoryPolicy.AddSlots(producer.Tier, 1);
        }

        private void Produce()
        {
            ProductionManager.Produce();
            Inventory.AddFunds(FundsChange.Value);
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

        private void IncreaseTier()
        {
            Tier.Value = (Tier)Math.Min((int)Tier.Value + 1, (int)Common.Types.Tier.Tier3);
        }

        private void OnMilestoneModifierAdded(IModifier modifier)
        {
            switch (modifier)
            {
                case MilestoneFundsBoostModifier:
                    FundsChange.AddModifier(modifier);
                    break;
            }
        }

        private void OnMilestoneModifierRemoved(IModifier modifier)
        {
            switch (modifier)
            {
                case MilestoneFundsBoostModifier:
                    FundsChange.RemoveModifier(modifier);
                    break;
            }
        }
    }
}