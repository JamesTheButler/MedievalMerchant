using Common.Infrastructure;
using Common.Infrastructure.Observation;

namespace Features.Trade.Logic
{
    public sealed class TradeService : IService
    {
        public ObservableEvent<TradeInfo> TradeCompleted { get; } = new();
        public ObservableEvent<TradeInfo> TradeAborted { get; } = new();

        public void Initialize() { }
        public void CleanUp() { }

        public void CompleteTrade(TradeInfo tradeInfo)
        {
            tradeInfo.Town.ResolveTrade(tradeInfo);
            TradeCompleted?.Invoke(tradeInfo);
        }
    }
}