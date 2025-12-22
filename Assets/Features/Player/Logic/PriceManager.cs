using System.Linq;
using Common.Infrastructure;
using Common.Infrastructure.Modifiable;
using Common.Types;
using Common.Utility;
using Features.Goods.Config;
using Features.Player.Retinue;
using Features.Player.Retinue.Logic;
using Features.Towns;
using Features.Towns.Development.Logic.Milestones;
using Features.Trade;
using Features.Trade.Logic;
using Features.Trade.Logic.Price;

namespace Features.Player.Logic
{
    public sealed class PriceManager
    {
        private readonly PlayerModel _player;
        private readonly Town _town;
        private readonly AvailabilityCalculator _availabilityCalculator;
        private readonly GoodsResources _goodsResources;
        private readonly GoodsConfig _goodsConfig;

        private AvailabilityPriceModifier _availabilityModifier;
        private readonly NegotiatorPriceModifier _negotiatorBuyModifier = new(0, TradeType.Buy);
        private readonly NegotiatorPriceModifier _negotiatorSellModifier = new(0, TradeType.Sell);
        private ReputationPriceModifier _reputationModifier;

        private Good _good;
        private TradeType _tradeType;

        // TODO: cache all created prices, later i can kill them again or create Handle<ModVar> or something

        public PriceManager(Town town)
        {
            _player = GameplayContext.Instance.Model.Player;
            _town = town;
            _goodsResources = ResourceManager.Instance.GoodsResources;
            _goodsConfig = ConfigurationManager.Configurations.GoodsConfig;
            _availabilityCalculator = new AvailabilityCalculator(town);
        }

        public ModifiableVariable GetPrice(Good good, TradeType tradeType)
        {
            _good = good;
            _tradeType = tradeType;

            var goodTier = _goodsResources.ConfigData[_good].Tier;
            var goodBasePrice = _goodsConfig.BasePriceData[goodTier];

            var basePriceModifier = new BasePriceModifier(goodBasePrice, goodTier);
            var price = new ModifiableVariable("Price per Good", tradeType == TradeType.Sell, basePriceModifier);

            AddAvailabilityModifier(price);
            AddRegionModifiers(price);
            AddReputationModifier();
            AddDevelopmentMilestoneModifiers(price);
            AddNegotiatorModifier(price, tradeType);

            _player.RetinueManager.CompanionLevels[CompanionType.Negotiator].Observe(OnCompanionChanged);
            _town.Inventory.GoodUpdated += OnTownInventoryChanged;
            _town.MilestoneManager.MilestoneModifierAdded += TownModifierAdded;
            _town.MilestoneManager.MilestoneModifierRemoved += TownModifierRemoved;
            _town.ReputationManager.Reputation.Observe(OnReputationChanged);
        }

        public void Clear()
        {
            _player.RetinueManager.CompanionLevels[CompanionType.Negotiator].StopObserving(OnCompanionChanged);
            _town.Inventory.GoodUpdated -= OnTownInventoryChanged;
            _town.MilestoneManager.MilestoneModifierAdded -= TownModifierAdded;
            _town.MilestoneManager.MilestoneModifierRemoved -= TownModifierRemoved;
            _town.ReputationManager.Reputation.StopObserving(OnReputationChanged);
        }

        #region Adding Modifiers

        private void AddAvailabilityModifier(ModifiableVariable price)
        {
            var availability = _availabilityCalculator.GetAvailability(_good);
            _availabilityModifier = new AvailabilityPriceModifier(availability);
            price.AddModifier(_availabilityModifier);
        }

        private void AddDevelopmentMilestoneModifiers(ModifiableVariable price)
        {
            var upgradeModifiers = _town.MilestoneManager.MilestoneModifiers.OfType<MilestonePriceBoostModifier>();
            foreach (var upgradeModifier in upgradeModifiers)
            {
                price.AddModifier(upgradeModifier);
            }
        }

        private void AddRegionModifiers(ModifiableVariable price)
        {
            // don't apply region modifier when buying from town
            if (_tradeType != TradeType.Sell)
                return;

            var goodRegions = _goodsResources.ConfigData[_good].Regions;
            var isLocal = _town.Regions.Intersects(goodRegions);

            IModifier regionModifier = isLocal
                ? new LocalGoodPriceModifier()
                : new ForeignGoodPriceModifier();
            price.AddModifier(regionModifier);
        }

        private void AddReputationModifier(ModifiableVariable price)
        {
            _reputationModifier = new ReputationPriceModifier(_town, _tradeType);
            price.AddModifier(_reputationModifier);
        }

        private void AddNegotiatorModifier(ModifiableVariable price, TradeType tradeType)
        {
            var negotiatorModifier = tradeType == TradeType.Buy ? _negotiatorBuyModifier : _negotiatorSellModifier;
            price.AddModifier(negotiatorModifier);
        }

        #endregion

        #region model change listeners

        private void OnCompanionChanged(int newLevel)
        {
            if (newLevel <= 0)
                return;

            _negotiatorBuyModifier.Update(newLevel);
            _negotiatorSellModifier.Update(newLevel);
        }

        private void OnTownInventoryChanged(Good good, int amount)
        {
            if (good != _good)
                return;

            var availability = _availabilityCalculator.GetAvailability(good);
            _availabilityModifier.Update(availability);
        }

        private void TownModifierAdded(IModifier modifier)
        {
            if (modifier is MilestonePriceBoostModifier)
                Price.AddModifier(modifier);
        }

        private void TownModifierRemoved(IModifier modifier)
        {
            if (modifier is MilestonePriceBoostModifier)
                Price.RemoveModifier(modifier);
        }

        private void OnReputationChanged(float reputation)
        {
            _reputationModifier.Update(reputation);
        }

        #endregion
    }
}