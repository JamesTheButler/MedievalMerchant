namespace Data
{
    public sealed class UnlimitedInventoryPolicy : IInventoryPolicy
    {
        public TradeResult CanAdd(Good good, int amount)
        {
            return TradeResult.Succeeded();
        }
    }
}