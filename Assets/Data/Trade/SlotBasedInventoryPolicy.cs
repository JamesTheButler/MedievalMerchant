using System;
using System.Collections.Generic;
using Data.Configuration;

namespace Data.Trade
{
    public sealed class SlotBasedInventoryPolicy : IInventoryPolicy
    {
        private readonly Lazy<GoodsConfig> _goodsConfig = new(() => ConfigurationManager.Instance.GoodsConfig);

        private readonly Dictionary<Tier, int> _slotsPerTier = new()
        {
            { Tier.Tier1, 0 },
            { Tier.Tier2, 0 },
            { Tier.Tier3, 0 },
        };

        private Inventory _inventory;

        public void SetInventory(Inventory inventory)
        {
            _inventory = inventory;
        }

        public void AddSlots(Tier tier, int amount)
        {
            _slotsPerTier[tier] += amount;
        }

        public TradeResult CanAdd(Good good, int amount)
        {
            var goodTier = _goodsConfig.Value.ConfigData[good].Tier;
            var slotsForThisGoodsTier = _inventory.GoodsPerTier()[goodTier];
            var canFitGood = _inventory.HasGood(good) || slotsForThisGoodsTier < _slotsPerTier[goodTier];

            return canFitGood
                ? TradeResult.Succeeded()
                : TradeResult.Failed($"There are no more empty slots for {goodTier}");
        }
    }
}