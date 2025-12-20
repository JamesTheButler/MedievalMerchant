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
            _model.TradesAborted.Value++;
        }

        private void OnTradeCompleted(TradeInfo info)
        {
            _model.TradesCompleted.Value++;
            _model.TradeVolumeTraded.Value += Mathf.Abs(info.TotalPrice);
            if (info.Type == TradeType.Buy)
            {
                _model.TotalValueBought.Value += Mathf.Abs(info.TotalPrice);
            }
            else
            {
                _model.TotalValueSold.Value += Mathf.Abs(info.TotalPrice);
            }
        }

        public void CleanUp() { }
    }
}