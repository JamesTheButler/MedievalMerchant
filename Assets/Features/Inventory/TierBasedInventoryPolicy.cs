using System;
using Common;
using Common.Types;
using Features.Goods.Config;
using Features.Trade;

namespace Features.Inventory
{
    public sealed class TierBasedInventoryPolicy : IInventoryPolicy
    {
        private readonly Lazy<GoodsConfig> _goodsConfig = new(() => ConfigurationManager.Instance.GoodsConfig);

        private Tier _maximumAllowedTier = Tier.Tier3; // by default, all tiers are allowed

        public void SetTier(Tier tier)
        {
            _maximumAllowedTier = tier;
        }

        public void SetInventory(Inventory inventory)
        {
            // not needed
        }

        public TradeResult CanAdd(Good good, int amount)
        {
            var goodTier = _goodsConfig.Value.ConfigData[good].Tier;

            return goodTier <= _maximumAllowedTier
                ? TradeResult.Succeeded()
                : TradeResult.Failed($"The Tier of the good is too high. Max. allowed Tier: {_maximumAllowedTier}");
        }
    }
}