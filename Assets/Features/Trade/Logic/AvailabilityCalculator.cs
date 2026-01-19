using Common.Infrastructure;
using Common.Types;
using Features.Goods.Config;
using Features.Towns;
using Features.Towns.Production.Config;

namespace Features.Trade.Logic
{
    public sealed class AvailabilityCalculator
    {
        private readonly Town _town;
        private readonly Inventory.Inventory _inventory;

        private readonly PriceModifierConfig _priceModifierConfig = ConfigurationManager.Configurations.PriceModifierConfig;
        private readonly ProducerConfig _townConfig = ConfigurationManager.Configurations.ProducerConfig;
        private readonly GoodResources _goodResources = ResourceManager.Instance.GoodResources;

        public AvailabilityCalculator(Town town)
        {
            _town = town;
            _inventory = town.Inventory;
        }

        public Availability GetAvailability(Good good)
        {
            var goodTier = _goodResources.ResourceData[good].Tier;
            // we use production limit for buy and sell right now
            var maxAmount = _townConfig.GetLimit(_town.Tier.Value, goodTier);

            var amount = _inventory.Get(good);
            var relativeAmount = (float)amount / maxAmount;

            if (amount <= 0)
                return Availability.VeryLow;

            // this assumes an order of the keys, which I know to work but it's not robust
            foreach (var marketState in _priceModifierConfig.AvailabilityConfigData.Keys)
            {
                var threshold = _priceModifierConfig.AvailabilityConfigData[marketState].ActivationThresholdInPercent / 100f;
                if (relativeAmount < threshold)
                {
                    return marketState - 1;
                }
            }

            return Availability.VeryHigh;
        }
    }
}