using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Features.Trade;
using Features.Trade.Logic;

namespace Features.Player.Logic
{
    public sealed class PlayerTradeTrackingSystem : ISystem
    {
        private TradeTracker _tradeTracker;
        private TradeService _tradeService;
        
        public void Initialize()
        {
            _tradeTracker = GameplayContext.Instance.Model.Player.TradeTracker;
            _tradeService = GameplayContext.Instance.Services.TradeService;

            _tradeService.TradeCompleted.Observe(OnTradeCompleted);
        }

        public void CleanUp()
        {
            _tradeService.TradeCompleted.StopObserving(OnTradeCompleted);
        }
        
        private void OnTradeCompleted(TradeInfo info)
        {
            if (info.Type == TradeType.Buy)
            {
                _tradeTracker.Add(info.Good, info.Amount, info.TotalPrice);
            }
            else
            {
                _tradeTracker.Remove(info.Good, info.Amount);
            }
        }

    }
}