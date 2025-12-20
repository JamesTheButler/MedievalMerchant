using Common.Types;

namespace Features.Trade
{
    public sealed record TradeInfo(TradeType Type, Good Good, int Amount, float TotalPrice, int HaggleLevel);
}