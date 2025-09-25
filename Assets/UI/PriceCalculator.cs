using System.Collections.Generic;
using Data;
using Data.Configuration;
using Data.Towns;
using Data.Trade;
using Data.Trade.Price;

namespace UI
{
    public sealed class PriceCalculator
    {
        private readonly Town _town;
        private readonly AvailabilityCalculator _availabilityCalculator;
        private readonly GoodsConfig _goodsConfig;

        public PriceCalculator(Town town)
        {
            _town = town;
            _goodsConfig = ConfigurationManager.Instance.GoodsConfig;
            _availabilityCalculator = new AvailabilityCalculator(town);
        }

        public Price GetPrice(Good good)
        {
            var tier = _goodsConfig.ConfigData[good].Tier;
            var basePrice = _goodsConfig.BasePriceData[tier];
            var modifiers = new List<PriceModifier>();

            var availability = _availabilityCalculator.GetAvailability(good);
            var multiplier = _availabilityCalculator.GetPriceMultiplier(good);
            modifiers.Add(new AvailabilityPriceModifier(multiplier, availability));

            return new Price(basePrice, modifiers);
        }
    }
}