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
using Features.Towns.Reputation.Logic;
using Features.Trade;
using Infrastructure;
using UnityEngine;

namespace Features.Towns
{
    public sealed class Town
    {
        private const int DefaultInventorySlots = 3;
        private const Tier StartTier = Common.Types.Tier.Tier1;

        public event Action<TradeInfo> TradeCompleted;

        public ProductionManager ProductionManager { get; }
        public DevelopmentManager DevelopmentManager { get; }
        public MilestoneManager MilestoneManager { get; }
        public ReputationManager ReputationManager { get; }
        public Inventory.Inventory Inventory { get; }
        public ModifiableVariable FundsChange { get; }

        public string Name { get; }
        public FlagInfo FlagInfo { get; private set; }
        public Vector2Int GridLocation { get; }
        public Vector2 WorldLocation { get; }
        public HashSet<Good> AvailableGoods { get; }
        public Region MainRegion { get; }
        public Regions Regions { get; }

        private readonly SlotBasedInventoryPolicy _inventoryPolicy;
        private readonly TownConfig _townConfig;
        private readonly GoodsResources _goodsResources;

        public IReadOnlyObservable<Tier> Tier => DevelopmentManager.Tier;

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

            _townConfig = ConfigurationManager.Configurations.TownConfig;
            var townResources = ResourceManager.Instance.TownResources;
            _goodsResources = ResourceManager.Instance.GoodsResources;
            AvailableGoods = availableGoods.ToHashSet();

            Name = townResources.NameGenerators[MainRegion].GenerateName();

            _inventoryPolicy.AddSlots(StartTier, DefaultInventorySlots);

            // initial funds and goods
            Inventory = new Inventory.Inventory(_inventoryPolicy);
            ProductionManager = new ProductionManager(this);
            DevelopmentManager = new DevelopmentManager(this);
            MilestoneManager = new MilestoneManager(this);
            ReputationManager = new ReputationManager(this);

            DevelopmentManager.Tier.Observe(OnTierChanged);
            ProductionManager.ProductionAdded += OnProductionManagerOnProductionAdded;
            MilestoneManager.MilestoneModifierAdded += OnMilestoneModifierAdded;
            MilestoneManager.MilestoneModifierRemoved += OnMilestoneModifierRemoved;

            Inventory.AddFunds(_townConfig.GetStartFunds());
            var baseModifier = new BaseTownFundsProduction(_townConfig.FundRate[StartTier], StartTier);
            FundsChange = new ModifiableVariable("Funds change per day", true, baseModifier);

            var startGood = AvailableGoods.GetRandom();
            AddProduction(startGood, 0);
            Inventory.AddGood(startGood, _townConfig.GetStartGoods());

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
            DevelopmentManager.Upgrade();
        }

        private void OnTierChanged(Tier tier)
        {
            _inventoryPolicy.AddSlots(tier, DefaultInventorySlots);
        }

        public void ResolveTrade(TradeInfo tradeInfo)
        {
            TradeCompleted?.Invoke(tradeInfo);
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

                var goodTier = _goodsResources.ConfigData[good].Tier;
                var consumptionRate = _townConfig.GetConsumptionRate(townTier, goodTier);

                if (consumptionRate == null)
                {
                    Debug.LogError($"No consumption rate is set for town {townTier} and good {goodTier}.");
                    continue;
                }

                Inventory.RemoveGood(good, consumptionRate.Value);
            }
        }

        private void OnMilestoneModifierAdded(IModifier modifier)
        {
            switch (modifier)
            {
                case MilestoneFundsBoostModifier:
                    FundsChange.AddModifier(modifier);
                    break;
                case MilestoneProductionBoostModifier prodBoostModifier:
                    ProductionManager.AddModifier(prodBoostModifier);
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
                case MilestoneProductionBoostModifier prodBoostModifier:
                    ProductionManager.RemoveModifier(prodBoostModifier);
                    break;
            }
        }
    }
}