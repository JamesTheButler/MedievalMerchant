using Common;
using Common.Types;
using Features.Trade;

namespace Features.Inventory
{
    public sealed class SlotCountInventoryPolicy : IInventoryPolicy
    {
        private readonly Observable<int> _slotCountObservable;

        private Inventory _inventory;

        public SlotCountInventoryPolicy(Observable<int> slotCountObservable)
        {
            _slotCountObservable = slotCountObservable;
        }

        public void SetInventory(Inventory inventory)
        {
            _inventory = inventory;
        }

        public TradeResult CanAdd(Good good, int amount)
        {
            var slotCount = _slotCountObservable.Value;
            var canAdd = _inventory.HasGood(good) || slotCount > _inventory.Goods.Count;
            return canAdd
                ? TradeResult.Succeeded()
                : TradeResult.Failed($"All {slotCount} inventory slots are full.");
        }
    }
}