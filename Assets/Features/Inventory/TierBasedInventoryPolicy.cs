using System;
using Common.Infrastructure;
using Common.Types;
using Features.Goods.Config;
using Features.Trade;

namespace Features.Inventory
{
    public sealed class TierBasedInventoryPolicy : IInventoryPolicy
    {
        private readonly Lazy<GoodResources> _goodsConfig = new(() => ResourceManager.Instance.GoodResources);

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
            var goodTier = _goodsConfig.Value.ResourceData[good].Tier;

            return goodTier <= _maxTier
                ? TradeResult.Succeeded()
                : TradeResult.Failed($"The Tier of the good is too high. Max. allowed Tier: {_maxTier}");
        }
    }
}