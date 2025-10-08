using Common;
using Data.Configuration;
using Data.Modifiable;
using Data.Player;
using Data.Player.Retinue;
using Data.Player.Retinue.Config;
using Data.Player.Retinue.Logic;
using Data.Towns;

namespace Data.Trade.Price
{
    public sealed class PriceCalculator
    {
        private readonly PlayerModel _player;
        private readonly Town _town;
        private readonly AvailabilityCalculator _availabilityCalculator;
        private readonly GoodsConfig _goodsConfig;
        private readonly CompanionConfig _companionConfig;

        public PriceCalculator(Town town)
        {
            _player = Model.Instance.Player;
            _town = town;
            _goodsConfig = ConfigurationManager.Instance.GoodsConfig;
            _companionConfig = ConfigurationManager.Instance.CompanionConfig;
            _availabilityCalculator = new AvailabilityCalculator(town);
        }

        public ModifiableVariable GetPrice(Good good, TradeType tradeType)
        {
            var goodTier = _goodsConfig.ConfigData[good].Tier;
            var goodRegions = _goodsConfig.ConfigData[good].Regions;
            var goodBasePrice = _goodsConfig.BasePriceData[goodTier];

            var basePriceModifier = new BasePriceModifier(goodBasePrice, goodTier);
            var price = new ModifiableVariable(basePriceModifier);

            AddAvailabilityModifier(good, price);
            ApplyRegionModifiers(tradeType, goodRegions, price);
            ApplyTownUpgrades(price);
            AddNegotiatorModifier(price, tradeType);

            return price;
        }

        private void AddAvailabilityModifier(Good good, ModifiableVariable price)
        {
            var availability = _availabilityCalculator.GetAvailability(good);
            var multiplier = _availabilityCalculator.GetPriceMultiplier(good);
            price.AddModifier(new AvailabilityPriceModifier(multiplier, availability));
        }

        private void ApplyTownUpgrades(ModifiableVariable price)
        {
            var upgradeModifiers = _town.UpgradeManager.ProductionModifiers;
            if (!upgradeModifiers.IsApproximately(0f))
            {
                price.AddModifier(new TownUpgradePriceModifier(upgradeModifiers));
            }
        }

        private void ApplyRegionModifiers(TradeType tradeType, Regions goodRegions, ModifiableVariable price)
        {
            // don't apply region modifier when buying from town
            if (tradeType != TradeType.Sell)
                return;

            var isLocal = _town.Regions.Intersects(goodRegions);
            IModifier regionModifier = isLocal
                ? new LocalGoodPriceModifier(_goodsConfig.LocalGoodPriceModifier)
                : new ForeignGoodPriceModifier(_goodsConfig.ForeignGoodPriceModifier);
            price.AddModifier(regionModifier);
        }

        private void AddNegotiatorModifier(ModifiableVariable price, TradeType tradeType)
        {
            var negotiatorLevel = _player.RetinueManager.CompanionLevels[CompanionType.Negotiator];
            if (negotiatorLevel == 0)
                return;

            var value = _companionConfig.NegotiatorData.GetTypedLevelData(negotiatorLevel).PriceSavings;
            var sign = tradeType == TradeType.Buy ? -1f : 1f;
            var priceModifier = new NegotiatorPriceModifier(sign * value, negotiatorLevel);

            price.AddModifier(priceModifier);
        }
    }
}