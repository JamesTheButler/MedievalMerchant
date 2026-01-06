using Common.Infrastructure;
using Features.Trade;
using Features.Trade.Logic;
using UnityEngine;

namespace Features.Stats
{
    public sealed class StatSystem : ISystem
    {
        private StatsModel _model;
        private TradeService _tradeService;

        public void Initialize()
        {
            _model = GameplayContext.Instance.Model.Stats;
            _tradeService = GameplayContext.Instance.Services.TradeService;
            _tradeService.TradeCompleted += OnTradeCompleted;
            _tradeService.TradeAborted += OnTradeAborted;
        }

        private void OnTradeAborted(TradeInfo info)
        {
            _model.TradesAborted++;
        }

        private void OnTradeCompleted(TradeInfo info)
        {
            _model.TradesCompleted++;
            if (info.Type == TradeType.Buy)
            {
                _model.TotalValueBought += Mathf.Abs(info.TotalPrice);
                _model.TrackBoughtGood(info.Good, info.Amount);
            }
            else
            {
                _model.TotalValueSold += Mathf.Abs(info.TotalPrice);
                _model.TrackSoldGood(info.Good, info.Amount);
            }
        }

        public void CleanUp() { }
    }
}