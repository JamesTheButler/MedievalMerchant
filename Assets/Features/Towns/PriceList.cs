using System;
using System.Collections.Generic;
using System.Linq;
using Common.Infrastructure;
using Common.Infrastructure.Modifiable;
using Common.Types;
using Features.Goods.Config;
using Features.Goods.Selector;
using Features.Trade;
using Features.Trade.Logic;
using Features.Trade.Logic.Price;

namespace Features.Towns
{
    public sealed class PriceList
    {
        private readonly TradeType _tradeType;
        private readonly Func<Good, bool> _pricePredicate;
        private readonly Dictionary<Good, ModifiableVariable> _cache = new();
        private readonly Dictionary<Good, AvailabilityPriceModifier> _availabilityModifiers = new();

        private readonly AvailabilityCalculator _availabilityCalculator;
        private readonly GoodsResources _goodsResources;
        private readonly GoodsConfig _goodsConfig;

        private readonly Dictionary<IModifier, IGoodSelector> _modifiers = new();

        public PriceList(TradeType tradeType, Town town, Func<Good, bool> pricePredicate)
        {
            _tradeType = tradeType;
            _pricePredicate = pricePredicate;
            _availabilityCalculator = new AvailabilityCalculator(town);
            _goodsResources = ResourceManager.Instance.GoodsResources;
            _goodsConfig = ConfigurationManager.Configurations.GoodsConfig;
        }

        public ModifiableVariable GetPrice(Good good)
        {
            if (!HasPrice(good))
                return null;

            if (_cache.TryGetValue(good, out var cachedPrice))
                return cachedPrice;

            var goodTier = _goodsResources.ConfigData[good].Tier;
            var goodBasePrice = _goodsConfig.BasePriceData[goodTier];
            var basePriceModifier = new BasePriceModifier(goodBasePrice, goodTier);

            var price = new ModifiableVariable(
                "Price per Good",
                _tradeType == TradeType.Sell,
                basePriceModifier);
            _cache.Add(good, price);

            // add availability
            var availabilityModifier = new AvailabilityPriceModifier(Availability.Normal);
            _availabilityModifiers.Add(good, availabilityModifier);
            RefreshAvailability(good);
            price.AddModifier(availabilityModifier);

            // add all other modifiers
            var matchingModifiers = _modifiers
                .Where(kv => kv.Value.Matches(good))
                .Select(kv => kv.Key);
            price.AddModifiers(matchingModifiers);

            return price;
        }

        public void AddModifier(IModifier modifier, IGoodSelector goodSelector)
        {
            _modifiers.Add(modifier, goodSelector);
        }

        public void RemoveModifier(IModifier modifier)
        {
            _modifiers.Remove(modifier);
        }

        public bool HasPrice(Good good)
        {
            return _pricePredicate.Invoke(good);
        }

        private void RefreshAvailability(Good good)
        {
            if (!_availabilityModifiers.ContainsKey(good))
                return;

            var availability = _availabilityCalculator.GetAvailability(good);
            _availabilityModifiers[good].Update(availability);
        }
    }
}