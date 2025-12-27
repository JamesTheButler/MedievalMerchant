using System;

namespace Features.Trade
{
    [Flags]
    public enum TradeTypes
    {
        None = 0,
        Buy = 1 << TradeType.Buy,
        Sell = 1 << TradeType.Sell,
        All = Buy | Sell
    }
}