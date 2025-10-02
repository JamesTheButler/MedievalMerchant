using Common;
using Data.Configuration;
using Data.Modifiable;
using Data.Towns;

namespace Data.Trade.Price
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

        public ModifiableVariable GetPrice(Good good, TradeType tradeType)
        {
            var goodTier = _goodsConfig.ConfigData[good].Tier;
            var goodRegions = _goodsConfig.ConfigData[good].Regions;
            var goodBasePrice = _goodsConfig.BasePriceData[goodTier];

            var basePriceModifier = new BasePriceModifier(goodBasePrice, goodTier);
            var price = new ModifiableVariable(basePriceModifier);

            var availability = _availabilityCalculator.GetAvailability(good);
            var multiplier = _availabilityCalculator.GetPriceMultiplier(good);
            price.AddModifier(new AvailabilityPriceModifier(multiplier, availability));

            // don't apply region modifier when buying from town
            if (tradeType == TradeType.Sell)
            {
                var isLocal = _town.Regions.Intersects(goodRegions);
                IModifier regionModifier = isLocal
                    ? new LocalGoodPriceModifier(_goodsConfig.LocalGoodPriceModifier)
                    : new ForeignGoodPriceModifier(_goodsConfig.ForeignGoodPriceModifier);
                price.AddModifier(regionModifier);
            }

            var upgradeModifiers = _town.UpgradeManager.ProductionModifiers;
            if (!upgradeModifiers.IsApproximately(0f))
            {
                price.AddModifier(new TownUpgradePriceModifier(upgradeModifiers));
            }

            return price;
        }
    }
}