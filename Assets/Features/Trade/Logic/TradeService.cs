using System;
using Common.Infrastructure;

namespace Features.Trade.Logic
{
    public sealed class TradeService : IService
    {
        public event Action<TradeInfo> TradeCompleted;
        public event Action<TradeInfo> TradeAborted;

        public void Initialize() { }
        public void CleanUp() { }

        public void CompleteTrade(TradeInfo tradeInfo)
        {
            tradeInfo.Town.ResolveTrade(tradeInfo);
            TradeCompleted?.Invoke(tradeInfo);
        }
    }
}