using Data.Configuration;
using Data.Trade;

namespace Data.Towns
{
    public sealed class MarketStateManager
    {
        private readonly Inventory _inventory;

        private readonly MarketStateConfig _marketStateConfig;

        public MarketStateManager(Inventory inventory)
        {
            _inventory = inventory;

            _marketStateConfig = ConfigurationManager.Instance.MarketStateConfig;
        }

        // TODO: clean up the copy pasta
        public MarketState GetMarketState(Good good)
        {
            var amount = _inventory.Get(good);
            if (amount < _marketStateConfig.ConfigData[MarketState.HighDemand].ActivationThreshold)
            {
                return MarketState.HighDemand;
            }

            if (amount < _marketStateConfig.ConfigData[MarketState.Demand].ActivationThreshold)
            {
                return MarketState.Demand;
            }

            if (amount < _marketStateConfig.ConfigData[MarketState.Supply].ActivationThreshold)
            {
                return MarketState.Supply;
            }

            return MarketState.HighSupply;
        }

        public float GetPriceMultiplier(Good good)
        {
            return _marketStateConfig.ConfigData[GetMarketState(good)].PriceMultiplier;
        }
    }
}