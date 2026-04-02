using System;
using Common.Infrastructure;
using Common.Types;
using Features.Localization.Data;
using Features.Trade;

namespace Features.Inventory
{
    public sealed class SlotCountInventoryPolicy : IInventoryPolicy
    {
        private int _slotCount;
        private Inventory _inventory;

        private readonly Lazy<TradeFailureStrings> _loc = new(() =>
            ResourceManager.Instance.LocalizationResources.Trade.FailureStrings);

        public SlotCountInventoryPolicy(int slotCount)
        {
            SetSlotCount(slotCount);
        }

        public void SetInventory(Inventory inventory)
        {
            _inventory = inventory;
        }

        public void SetSlotCount(int slotCount)
        {
            _slotCount = slotCount;
        }

        public TradeResult CanAdd(Good good, int amount)
        {
            var canAdd = _inventory.HasGood(good) || _slotCount > _inventory.Goods.Count;
            return canAdd
                ? TradeResult.Succeeded()
                : TradeResult.Failed(_loc.Value.InsufficientSpace());
        }
    }
}