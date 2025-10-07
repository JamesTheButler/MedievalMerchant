using Data.Configuration;
using Data.Towns;
using Data.Towns.Production.Config;

namespace Data.Trade
{
    public sealed class AvailabilityCalculator
    {
        private readonly Town _town;
        private readonly Inventory _inventory;

        private readonly AvailabilityConfig _availabilityConfig = ConfigurationManager.Instance.AvailabilityConfig;
        private readonly ProducerConfig _townConfig = ConfigurationManager.Instance.ProducerConfig;
        private readonly GoodsConfig _goodsConfig = ConfigurationManager.Instance.GoodsConfig;

        public AvailabilityCalculator(Town town)
        {
            _town = town;
            _inventory = town.Inventory;
        }

        public Availability GetAvailability(Good good)
        {
            var goodTier = _goodsConfig.ConfigData[good].Tier;
            // we use production limit for buy and sell right now
            var maxAmount = _townConfig.GetLimit(_town.Tier, goodTier);

            var amount = _inventory.Get(good);
            var relativeAmount = (float)amount / maxAmount;

            if (amount <= 0)
                return Availability.VeryLow;

            // this assumes an order of the keys, which I know to work but it's not robust
            foreach (var marketState in _availabilityConfig.ConfigData.Keys)
            {
                var threshold = _availabilityConfig.ConfigData[marketState].ActivationThresholdInPercent / 100f;
                if (relativeAmount < threshold)
                {
                    return marketState - 1;
                }
            }

            return Availability.VeryHigh;
        }

        public float GetPriceMultiplier(Good good)
        {
            return _availabilityConfig.ConfigData[GetAvailability(good)].PriceMultiplier;
        }
    }
}