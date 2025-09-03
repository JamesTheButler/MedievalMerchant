namespace Data.Trade
{
    public interface IInventoryPolicy
    {
        void SetInventory(Inventory inventory);
        TradeResult CanAdd(Good good, int amount);
    }
}