using System;
using System.Collections.Generic;
using System.Linq;
using Common.Infrastructure;
using Common.Infrastructure.Modifiable;
using Common.Infrastructure.Observation;
using Common.Types;
using Common.Utility;
using Features.Inventory;
using Features.Towns.Development.Logic;
using Features.Towns.Development.Logic.Milestones;
using Features.Towns.Flags;
using Features.Towns.Flags.Logic;
using Features.Towns.Production.Logic;
using Features.Towns.Reputation.Logic;
using Features.Trade;
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
        public ReputationManager ReputationManager { get; }
        public Inventory.Inventory Inventory { get; }
        public ModifiableVariable FundsChange { get; }
        public PriceManager PriceManager { get; }
        public MilestoneModel Milestones { get; }
        public MissionModel Missions { get; }

        public string Name { get; }
        public FlagInfo FlagInfo { get; private set; }
        public Vector2Int GridLocation { get; }
        public Vector2 WorldLocation { get; }
        public HashSet<Good> AvailableGoods { get; }
        public Region MainRegion { get; }
        public Regions Regions { get; }

        // TODO - Feature: each good needs an Observable<float> consumption rate once implement consumption modifiers
        public Observable<float> ConsumptionRate { get; }

        public IReadOnlyObservable<Tier> Tier => DevelopmentManager.Tier;

        private readonly SlotBasedInventoryPolicy _inventoryPolicy;

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

            var townConfig = ConfigurationManager.Configurations.TownConfig;
            var townResources = ResourceManager.Instance.TownResources;
            AvailableGoods = availableGoods.ToHashSet();

            Name = townResources.NameGenerators[MainRegion].GenerateName();

            _inventoryPolicy.AddSlots(StartTier, DefaultInventorySlots);

            // initial funds and goods
            Inventory = new Inventory.Inventory(_inventoryPolicy);
            ProductionManager = new ProductionManager(this);
            DevelopmentManager = new DevelopmentManager(this);
            ReputationManager = new ReputationManager(this);
            PriceManager = new PriceManager(this);
            Milestones = new MilestoneModel();

            const Tier tempTier = Common.Types.Tier.Tier1;
            var consumptionRate = townConfig.GetConsumptionRate(tempTier, tempTier) ?? 0f;
            ConsumptionRate = new Observable<float>(consumptionRate);

            DevelopmentManager.Tier.Observe(OnTierChanged);
            ProductionManager.ProductionAdded += OnProductionManagerOnProductionAdded;

            Inventory.AddFunds(townConfig.GetStartFunds());
            var baseModifier = new BaseTownFundsProduction(townConfig.FundRate[StartTier], StartTier);
            FundsChange = new ModifiableVariable("Funds change per day", true, baseModifier);

            var startGood = AvailableGoods.GetRandom();
            AddProduction(startGood, 0);
            Inventory.AddGood(startGood, townConfig.GetStartGoods());

            FlagInfo = flagFactory.CreateFlagInfo(MainRegion);
        }

        public void AddProduction(Good good, int index)
        {
            ProductionManager.AddProducer(good, index);
        }

        public void ResolveTrade(TradeInfo tradeInfo)
        {
            TradeCompleted?.Invoke(tradeInfo);
        }

        private void OnTierChanged(Tier tier)
        {
            _inventoryPolicy.AddSlots(tier, DefaultInventorySlots);
        }

        private void OnProductionManagerOnProductionAdded(Producer producer)
        {
            _inventoryPolicy.AddSlots(producer.Tier, 1);
        }
    }
}