using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Infrastructure.Observation;
using Features.Trade;
using Features.Trade.Logic;
using UnityEngine;

namespace Features.Stats
{
    public sealed class StatSystem : ISystem
    {
        private readonly Bindings _bindings = new();

        private StatsModel _model;
        private TradeService _tradeService;

        public void Initialize()
        {
            _model = GameplayContext.Instance.Model.Stats;
            _tradeService = GameplayContext.Instance.Services.TradeService;

            _bindings.Track(
                _tradeService.TradeCompleted.Observe(OnTradeCompleted),
                _tradeService.TradeAborted.Observe(OnTradeAborted)
            );
        }

        public void CleanUp()
        {
            _bindings.UnbindAll();
        }

        private void OnTradeAborted()
        {
            _model.TradesAborted++;
        }

        private void OnTradeCompleted(OngoingTrade trade)
        {
            _model.TradesCompleted++;
            if (trade.TradeType == TradeType.Buy)
            {
                _model.TotalValueBought += Mathf.Abs(trade.TotalPrice);
                _model.TrackBoughtGood(trade.Good, trade.Amount);
            }
            else
            {
                _model.TotalValueSold += Mathf.Abs(trade.TotalPrice);
                _model.TrackSoldGood(trade.Good, trade.Amount);
            }
        }
    }
}