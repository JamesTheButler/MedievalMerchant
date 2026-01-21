using Common.Types;
using Features.Towns;
using Features.Trade.Haggling;

namespace Features.Trade
{
    public sealed record TradeInfo(Town Town, TradeType Type, Good Good, int Amount, float TotalPrice, HaggleLevel HaggleLevel);
}