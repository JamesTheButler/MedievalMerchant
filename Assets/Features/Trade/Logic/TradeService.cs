using Common.Infrastructure;
using Common.Infrastructure.Observation;
using UnityEngine;

namespace Features.Trade.Logic
{
    public sealed class TradeService : IService
    {
        public IReadOnlyObservableEvent<TradeInfo> TradeCompleted => _tradeCompleted;
        public IReadOnlyObservableEvent TradeAborted => _tradeAborted;

        private readonly ObservableEvent<TradeInfo> _tradeCompleted = new();
        private readonly ObservableEvent _tradeAborted = new();

        public void Initialize() { }
        public void CleanUp() { }

        public void CompleteTrade(TradeInfo tradeInfo)
        {
            tradeInfo.Town.ResolveTrade(tradeInfo);
            _tradeCompleted?.Invoke(tradeInfo);
            
            Debug.Log($"Trade completed. Info: {tradeInfo}.");
        }

        public void AbortTrade()
        {
            _tradeAborted.Invoke();
            Debug.Log("Trade aborted.");
        }
    }
}