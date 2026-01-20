using System.Collections.Generic;
using System.Linq;
using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Infrastructure.Observation;
using Common.Types;
using Features.Goods.Selector;
using Features.Towns;

namespace Features.Trade.Logic.Price
{
    public sealed class DateLog<T> : Dictionary<Date, T> { }

    public sealed class DisinterestSystem : ISystem
    {
        private readonly Town _town;
        private readonly Dictionary<Good, DisinterestPriceModifier> _modifiers = new();
        private readonly Dictionary<Good, DateLog<int>> _goodLogs = new();
        private readonly Bindings _bindings = new();

        private TradeService _tradeService;
        private Date _gameDate;
        private DisinterestModiferConfigData _config;

        public DisinterestSystem(Town town)
        {
            _town = town;
        }

        public void Initialize()
        {
            _tradeService = GameplayContext.Instance.Services.TradeService;
            _gameDate = GameplayContext.Instance.Model.Date;
            _config = ConfigurationManager.Configurations.PriceModifierConfig.DisinterestModiferConfig;

            _bindings.Track(
                _tradeService.TradeCompleted.Observe(OnTradeCompleted),
                _gameDate.Changed.Observe(OnDateChanged)
            );
        }

        public void CleanUp()
        {
            _bindings.UnbindAll();
        }

        private void OnTradeCompleted(TradeInfo tradeInfo)
        {
            if (tradeInfo.Town != _town)
                return;

            var good = tradeInfo.Good;
            _goodLogs.TryAdd(good, new DateLog<int>());
            var dateToDeactivate = _gameDate + _config.TrackedPeriodInDays;
            _goodLogs[good].TryAdd(dateToDeactivate, 0);
            _goodLogs[good][dateToDeactivate] += tradeInfo.Amount;

            UpdateModifier(good);
        }

        private void UpdateModifier(Good good)
        {
            var trackedSum = GetTrackedSum(good);

            DisinterestPriceModifier modifier;
            if (_modifiers.ContainsKey(good))
            {
                modifier = _modifiers[good];
            }
            else
            {
                modifier = new DisinterestPriceModifier(good, 0);
                _modifiers.Add(good, modifier);
                _town.PriceManager.AddModifier(modifier, new SingleGoodSelector(good), TradeType.Sell);
            }

            modifier.Update(trackedSum);
        }

        private void OnDateChanged(Date date)
        {
            foreach (var (good, log) in _goodLogs)
            {
                if (log.Any(l => l.Key == date))
                {
                    log.Remove(date);
                    UpdateModifier(good);

                    //if (log.Count == 0)
                    //{
                    //    _town.PriceManager.RemoveModifier(_modifiers[good], TradeType.Sell);
                    //    _modifiers.Remove(good);
                    //}
                }
            }
        }

        private int GetTrackedSum(Good good)
        {
            return _goodLogs.TryGetValue(good, out var log) ? log.Values.Sum() : 0;
        }
    }
}