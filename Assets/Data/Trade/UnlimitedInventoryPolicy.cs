namespace Data.Trade
{
    public sealed class UnlimitedInventoryPolicy : IInventoryPolicy
    {
        public void SetInventory(Inventory inventory)
        {
            // not relevant
        }

        public TradeResult CanAdd(Good good, int amount)
        {
            return TradeResult.Succeeded();
        }
    }
}