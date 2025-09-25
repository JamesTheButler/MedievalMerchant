using System.Collections.Generic;
using Common;
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

        public Price GetPrice(Good good, TradeType tradeType)
        {
            var goodTier = _goodsConfig.ConfigData[good].Tier;
            var goodRegions = _goodsConfig.ConfigData[good].Regions;
            var goodBasePrice = _goodsConfig.BasePriceData[goodTier];

            var modifiers = new List<PriceModifier>();
            var availability = _availabilityCalculator.GetAvailability(good);
            var multiplier = _availabilityCalculator.GetPriceMultiplier(good);
            modifiers.Add(new AvailabilityPriceModifier(multiplier, availability));

            // don't apply region modifier when buying from town
            if (tradeType == TradeType.Sell)
            {
                var isLocal = _town.Regions.Intersects(goodRegions);
                PriceModifier regionModifier = isLocal
                    ? new LocalGoodPriceModifier(_goodsConfig.LocalGoodPriceModifier)
                    : new ForeignGoodPriceModifier(_goodsConfig.ForeignGoodPriceModifier);
                modifiers.Add(regionModifier);
            }

            return new Price(goodBasePrice, modifiers);
        }
    }
}