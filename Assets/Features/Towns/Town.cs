using System;
using System.Collections.Generic;
using System.Linq;
using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Infrastructure.Modifiable;
using Common.Infrastructure.Observation;
using Common.Types;
using Common.Utility;
using Features.Goods.Config;
using Features.Inventory;
using Features.Towns.Config;
using Features.Towns.Development.Logic;
using Features.Towns.Development.Logic.Milestones;
using Features.Towns.Flags;
using Features.Towns.Flags.Logic;
using Features.Towns.Missions;
using Features.Towns.Production.Logic;
using Features.Towns.Reputation.Logic;
using Features.Trade;
using Features.Trade.Logic.Price;
using UnityEngine;

namespace Features.Towns
{
    public sealed class Town
    {
        private const Tier StartTier = Common.Types.Tier.Tier1;

        public ProductionManager ProductionManager { get; }
        public DevelopmentManager DevelopmentManager { get; }
        public ReputationModel ReputationModel { get; }
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
        public Observable<string> Descriptor { get; } = new("Town");

        public IReadOnlyObservable<Tier> Tier => DevelopmentManager.Tier;

        private readonly SlotBasedInventoryPolicy _inventoryPolicy;
        private readonly RecipeResources _recipeResources;
        private readonly TownConfig _townConfig;

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
            _recipeResources = ResourceManager.Instance.RecipeResources;
            AvailableGoods = availableGoods.ToHashSet();

            Name = townResources.NameGenerators[MainRegion].GenerateName();

            _inventoryPolicy.AddSlots(StartTier, _townConfig.InventorySlotsPerTier[StartTier]);

            // initial funds and goods
            Inventory = new Inventory.Inventory(_inventoryPolicy);
            ProductionManager = new ProductionManager(this);
            DevelopmentManager = new DevelopmentManager(this);
            ReputationModel = new ReputationModel();
            PriceManager = new PriceManager(this);
            Milestones = new MilestoneModel();
            Missions = new MissionModel();

            const Tier tempTier = Common.Types.Tier.Tier1;
            var consumptionRate = _townConfig.GetConsumptionRate(tempTier, tempTier) ?? 0f;
            ConsumptionRate = new Observable<float>(consumptionRate);

            DevelopmentManager.Tier.Observe(OnTierChanged);
            ProductionManager.ProductionAdded += OnProductionManagerOnProductionAdded;

            Inventory.AddFunds(_townConfig.GetStartFunds());
            var baseModifier = new BaseTownFundsProduction(_townConfig.FundRate[StartTier], StartTier);
            FundsChange = new ModifiableVariable("Funds change per day", true, baseModifier);

            var startGood = AvailableGoods.GetRandom();
            AddProduction(startGood, 0);
            Inventory.AddGood(startGood, _townConfig.GetStartGoods());

            FlagInfo = flagFactory.CreateFlagInfo(MainRegion);
        }

        public void AddProduction(Good good, int index)
        {
            ProductionManager.AddProducer(good, index);
        }

        private void OnTierChanged(Tier tier)
        {
            _inventoryPolicy.AddSlots(tier, _townConfig.InventorySlotsPerTier[tier]);
            switch (tier)
            {
                case Common.Types.Tier.Tier2:
                    var t2Goods = new List<Good>();
                    foreach (var good in AvailableGoods)
                    {
                        t2Goods.Add(_recipeResources.GetTier2RecipeForComponent(good).Result);
                    }

                    AvailableGoods.Add(t2Goods);
                    break;

                case Common.Types.Tier.Tier3:
                    var t3Goods = new List<Good>();
                    var globalGoodPool = GameplayContext.Instance.Model.GoodPool;
                    foreach (var t3Good in globalGoodPool.Tier3Goods)
                    {
                        var t3Recipe = _recipeResources.GetTier3RecipeForResult(t3Good);
                        if (AvailableGoods.Contains(t3Recipe.Component1) ||
                            AvailableGoods.Contains(t3Recipe.Component2))
                        {
                            t3Goods.Add(t3Good);
                        }
                    }

                    AvailableGoods.Add(t3Goods);
                    break;
            }
        }

        private void OnProductionManagerOnProductionAdded(Producer producer)
        {
            _inventoryPolicy.AddSlots(producer.Tier, 1);
        }
    }
}