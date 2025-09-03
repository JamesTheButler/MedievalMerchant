namespace Data
{
    public interface IInventoryPolicy
    {
        TradeResult CanAdd(Good good, int amount);
    }
}