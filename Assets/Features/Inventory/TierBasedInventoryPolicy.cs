using System;
using Common;
using Common.Types;
using Features.Goods.Config;
using Features.Trade;
using Infrastructure;

namespace Features.Inventory
{
    public sealed class TierBasedInventoryPolicy : IInventoryPolicy
    {
        private readonly Lazy<GoodsResources> _goodsConfig = new(() => ResourceManager.Instance.GoodsResources);

        private Tier _maxTier = Tier.Tier3; // by default, all tiers are allowed

        public void SetTier(Tier tier)
        {
            _maxTier = tier;
        }

        public void SetInventory(Inventory inventory)
        {
            // not needed
        }

        public TradeResult CanAdd(Good good, int amount)
        {
            var goodTier = _goodsConfig.Value.ConfigData[good].Tier;

            return goodTier <= _maxTier
                ? TradeResult.Succeeded()
                : TradeResult.Failed($"The Tier of the good is too high. Max. allowed Tier: {_maxTier}");
        }
    }
}