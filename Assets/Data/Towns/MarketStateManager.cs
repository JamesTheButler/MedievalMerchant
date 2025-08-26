using Data.Setup;

namespace Data.Towns
{
    public sealed class MarketStateManager
    {
        private readonly Inventory _inventory;

        private readonly MarketStateMultipliers _marketStateMultipliers;
        private readonly MarketStateThresholds _marketStateThresholds;

        public MarketStateManager(Inventory inventory)
        {
            _inventory = inventory;
            _marketStateMultipliers = SetupManager.Instance.MarketStateMultipliers;
            _marketStateThresholds = SetupManager.Instance.MarketStateThresholds;
        }

        // TODO: clean up the copy pasta
        public MarketState GetMarketState(Good good)
        {
            var amount = _inventory.Get(good);
            if (amount < _marketStateThresholds.Thresholds[MarketState.HighDemand])
            {
                return MarketState.HighDemand;
            }

            if (amount < _marketStateThresholds.Thresholds[MarketState.Demand])
            {
                return MarketState.Demand;
            }

            if (amount < _marketStateThresholds.Thresholds[MarketState.Supply])
            {
                return MarketState.Supply;
            }


            return MarketState.HighSupply;
        }

        public float GetPriceMultiplier(Good good)
        {
            return _marketStateMultipliers.Multipliers[GetMarketState(good)];
        }
    }
}