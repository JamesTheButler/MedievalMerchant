using Common.Types;
using Features.Towns;

namespace Features.Trade
{
    public sealed record TradeInfo(Town Town, TradeType Type, Good Good, int Amount, float TotalPrice, int HaggleLevel);
}