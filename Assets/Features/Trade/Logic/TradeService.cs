using System.Collections.Generic;
using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Infrastructure.Observation;
using Common.Types;
using Features.Goods.Config;
using Features.Player.Logic;
using Features.Towns;
using Features.Trade.Haggling.Data;
using UnityEngine;

namespace Features.Trade.Logic
{
    public sealed class TradeService : IService
    {
        public IReadOnlyObservableEvent<OngoingTrade> TradeCompleted => _tradeCompleted;
        public IReadOnlyObservableEvent TradeAborted => _tradeAborted;

        private readonly ObservableEvent<OngoingTrade> _tradeCompleted = new();
        private readonly ObservableEvent _tradeAborted = new();
        private readonly Dictionary<OngoingTrade, Bindings> _bindings = new();

        private GameplayModel _model;
        private PlayerModel _player;
        private GoodResources _goodResources;
        private HaggleResources _haggleResources;

        public void Initialize()
        {
            _model = GameplayContext.Instance.Model;
            _player = _model.Player;
            _goodResources = ResourceManager.Instance.GoodResources;
            _haggleResources = ResourceManager.Instance.HaggleResources;
        }

        public void CleanUp() { }

        public OngoingTrade InitializeTrade(Town town, Good good, TradeType tradeType)
        {
            var trade = new OngoingTrade(town, good, tradeType);
            trade.Initialize();

            var bindings = new Bindings();
            bindings.Track(
                trade.Completed.Observe(() => OnTradeCompleted(trade)),
                trade.Aborted.Observe(() => OnTradeAborted(trade))
            );
            _bindings.Add(trade, bindings);

            return trade;
        }

        private void OnTradeCompleted(OngoingTrade trade)
        {
            UpdateTownReputation(trade);
            UpdateInventories(trade);


            var bindings = _bindings[trade];
            bindings.UnbindAll();
            _bindings.Remove(trade);

            _tradeCompleted?.Invoke(trade);

            Debug.Log($"Trade completed: {trade}.");
        }

        private void UpdateInventories(OngoingTrade trade)
        {
            var buyer = trade.TradeType == TradeType.Buy ? _player.Inventory : trade.Town.Inventory;
            var seller = trade.TradeType != TradeType.Buy ? _player.Inventory : trade.Town.Inventory;

            buyer.RemoveFunds(trade.TotalPrice);
            seller.AddFunds(trade.TotalPrice);

            buyer.AddGood(trade.Good, trade.Amount);
            seller.RemoveGood(trade.Good, trade.Amount);
        }

        private void UpdateTownReputation(OngoingTrade trade)
        {
            var good = _goodResources.ResourceData[trade.Good].GoodName;
            var haggleLevel = _haggleResources.HaggleLevelNames[trade.HaggleLevel];
            var message = $"Traded {trade.Amount}x{good} worth {trade.TotalPrice} coin, haggling {haggleLevel}ly.";

            trade.Town.ReputationManager.UpdateReputation(trade.ReputationChange, message);
        }

        private void OnTradeAborted(OngoingTrade trade)
        {
            Debug.Log("Trade aborted.");

            var bindings = _bindings[trade];
            bindings.UnbindAll();
            _bindings.Remove(trade);

            _tradeAborted.Invoke();
        }
    }
}