namespace Features.Trade.Logic
{
    public static class TradeExtensions
    {
        public static CompletedTrade AsCompleted(this OngoingTrade trade)
        {
            return new CompletedTrade(
                trade.Town,
                trade.TradeType,
                trade.Good,
                trade.Amount,
                trade.TotalPrice,
                trade.HaggleLevel);
        }
    }
}