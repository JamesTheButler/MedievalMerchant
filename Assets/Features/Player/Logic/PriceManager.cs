using System.Collections.Generic;
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

        private readonly NegotiatorPriceModifier _negotiatorBuyModifier = new(0, TradeType.Buy);
        private readonly NegotiatorPriceModifier _negotiatorSellModifier = new(0, TradeType.Sell);
        private ReputationPriceModifier _reputationBuyModifier;
        private ReputationPriceModifier _reputationSellModifier;

        // TODO: cache all created prices, later i can kill them again or create Handle<ModVar> or something

        // good specific modifiers
        // availabilty
        // region ==> depends on buy sell
        // town specific modifiers
        // reputation
        // town milestones
        // negotiator ==> depends on buy/sell
        // town missions
        // level modifiers
        // events

        private readonly List<IModifier> _milestoneModifiers = new();
        private readonly Dictionary<Good, ModifiableVariable> _prices = new();

        public PriceManager(Town town)
        {
            _player = GameplayContext.Instance.Model.Player;
            _town = town;
            _goodsResources = ResourceManager.Instance.GoodsResources;
            _goodsConfig = ConfigurationManager.Configurations.GoodsConfig;
            _availabilityCalculator = new AvailabilityCalculator(town);

            _reputationBuyModifier = new ReputationPriceModifier(town, TradeType.Buy);
            _reputationBuyModifier = new ReputationPriceModifier(town, TradeType.Sell);
        }

        public ModifiableVariable GetPrice(Good good)
        {
            if (_prices.TryGetValue(good, out var cachedPrice))
                return cachedPrice;

            //TODO: THIS IS NOT CORRECT!!!
            var tradeType = TradeType.Buy;

            var goodTier = _goodsResources.ConfigData[good].Tier;
            var goodBasePrice = _goodsConfig.BasePriceData[goodTier];

            var basePriceModifier = new BasePriceModifier(goodBasePrice, goodTier);
            var price = new ModifiableVariable("Price per Good", tradeType == TradeType.Sell, basePriceModifier);

            AddAvailabilityModifier(price, good);
            AddRegionModifiers(price, good, tradeType);
            AddReputationModifier(price, tradeType);
            AddDevelopmentMilestoneModifiers(price);
            AddNegotiatorModifier(price, tradeType);

            _prices.Add(good, price);
            return price;
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

        private void AddAvailabilityModifier(ModifiableVariable price, Good good)
        {
            var availability = _availabilityCalculator.GetAvailability(good);
            var availabilityModifier = new AvailabilityPriceModifier(availability);
            price.AddModifier(availabilityModifier);
        }

        private void AddDevelopmentMilestoneModifiers(ModifiableVariable price)
        {
            var upgradeModifiers = _town.MilestoneManager.MilestoneModifiers.OfType<MilestonePriceBoostModifier>();
            foreach (var upgradeModifier in upgradeModifiers)
            {
                price.AddModifier(upgradeModifier);
            }
        }

        private void AddRegionModifiers(ModifiableVariable price, Good good, TradeType tradeType)
        {
            // don't apply region modifier when buying from town
            if (tradeType != TradeType.Sell)
                return;

            var goodRegions = _goodsResources.ConfigData[good].Regions;
            var isLocal = _town.Regions.Intersects(goodRegions);

            IModifier regionModifier = isLocal
                ? new LocalGoodPriceModifier()
                : new ForeignGoodPriceModifier();
            price.AddModifier(regionModifier);
        }

        private void AddReputationModifier(ModifiableVariable price, TradeType tradeType)
        {
            price.AddModifier(tradeType == TradeType.Buy ? _reputationBuyModifier : _reputationSellModifier);
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
            if (!_prices.TryGetValue(good, out var price))
                return;

            // TODO: meh
            var availabilityModifier = price.Modifiers.FirstOfType<AvailabilityPriceModifier, IModifier>();
            var availability = _availabilityCalculator.GetAvailability(good);
            availabilityModifier.Update(availability);
        }

        private void TownModifierAdded(IModifier modifier)
        {
            if (modifier is not MilestonePriceBoostModifier)
                return;

            _milestoneModifiers.Add(modifier);
            foreach (var price in _prices.Values)
            {
                price.AddModifier(modifier);
            }
        }

        private void TownModifierRemoved(IModifier modifier)
        {
            if (modifier is not MilestonePriceBoostModifier)
                return;

            _milestoneModifiers.Remove(modifier);
            foreach (var price in _prices.Values)
            {
                price.RemoveModifier(modifier);
            }
        }

        private void OnReputationChanged(float reputation)
        {
            _reputationBuyModifier.Update(reputation);
            _reputationSellModifier.Update(reputation);
        }

        #endregion
    }
}