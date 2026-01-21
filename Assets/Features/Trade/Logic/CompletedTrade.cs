using Common.Types;
using Features.Towns;
using Features.Trade.Haggling;

namespace Features.Trade.Logic
{
    public sealed record CompletedTrade(
        Town Town,
        TradeType TradeType,
        Good Good,
        int Amount,
        float TotalPrice,
        HaggleLevel HaggleLevel);
}