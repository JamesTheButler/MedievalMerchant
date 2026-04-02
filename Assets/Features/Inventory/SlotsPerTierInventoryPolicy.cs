using System;
using System.Collections.Generic;
using Common.Infrastructure;
using Common.Types;
using Features.Goods.Config;
using Features.Localization.Data;
using Features.Trade;

namespace Features.Inventory
{
    public sealed class SlotsPerTierInventoryPolicy : IInventoryPolicy
    {
        private readonly Lazy<GoodResources> _goodResources = new(() => ResourceManager.Instance.GoodResources);

        private readonly Lazy<TradeFailureStrings> _loc = new(() =>
            ResourceManager.Instance.LocalizationResources.Trade.FailureStrings);

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
            var goodTier = _goodResources.Value.ResourceData[good].Tier;
            var slotsForThisGoodsTier = _inventory.GoodsPerTier()[goodTier];
            var canFitGood = _inventory.HasGood(good) || slotsForThisGoodsTier < _slotsPerTier[goodTier];

            return canFitGood
                ? TradeResult.Succeeded()
                : TradeResult.Failed(_loc.Value.InsufficientSlots(goodTier));
        }
    }
}