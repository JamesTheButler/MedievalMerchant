using Common.Types;
using Features.Trade;

namespace Features.Inventory
{
    public interface IInventoryPolicy
    {
        void SetInventory(Inventory inventory);
        TradeResult CanAdd(Good good, int amount);
    }
}