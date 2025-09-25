using System;

namespace Data.Trade
{
    public enum TradeType
    {
        Buy,
        Sell,
    }

    public static class TradeTypeExtensions
    {
        public static string ToDisplayString(this TradeType tradeType)
        {
            return tradeType switch
            {
                TradeType.Buy => "Buy",
                TradeType.Sell => "Sell",
                _ => throw new ArgumentOutOfRangeException(nameof(tradeType), tradeType, null)
            };
        }
    }
}