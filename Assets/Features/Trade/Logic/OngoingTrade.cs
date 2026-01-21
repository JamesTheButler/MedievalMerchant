using System.Collections.Generic;
using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Infrastructure.Modifiable;
using Common.Infrastructure.Observation;
using Common.Types;
using Features.Player.Logic;
using Features.Towns;
using Features.Trade.Haggling;
using Features.Trade.Haggling.Data;
using UnityEngine;

namespace Features.Trade.Logic
{
    public sealed class OngoingTrade : IInitializable
    {
        private readonly ModifiableVariable _singlePrice;

        public Good Good { get; }
        public Town Town { get; }
        public TradeType TradeType { get; }
        public HaggleLevel HaggleLevel { get; private set; }

        public ModifiableVariable SinglePrice => _singlePrice;
        public Observable<float> TotalPrice { get; } = new();
        public Observable<float> ReputationChange { get; } = new();
        public Observable<float?> Profit { get; } = new();
        public Observable<int> Amount { get; } = new();

        public ObservableEvent Completed { get; } = new();
        public ObservableEvent Aborted { get; } = new();

        private GameplayModel _model;
        private PlayerModel _player;
        private TradeTracker _tradeTracker;

        private HaggleConfig _haggleConfig;
        private readonly HagglePriceModifier _hagglePriceModifier;

        public OngoingTrade(
            Town town,
            Good good,
            TradeType tradeType)
        {
            Good = good;
            TradeType = tradeType;
            Town = town;

            _hagglePriceModifier = new HagglePriceModifier(HaggleLevel.Kind, TradeType);
            _singlePrice = town.PriceManager.GetPrice(good, tradeType).Copy();
            _singlePrice.AddModifier(_hagglePriceModifier);
        }

        public void Initialize()
        {
            _haggleConfig = ConfigurationManager.Configurations.HaggleConfig;
            _model = GameplayContext.Instance.Model;
            _player = _model.Player;
            _tradeTracker = _player.TradeTracker;
            
            _singlePrice.Observe(RefreshProfit, true);
        }

        public void CleanUp()
        {
            _singlePrice.StopObserving(RefreshProfit);
        }

        public void Complete()
        {
            Completed.Invoke();
            CleanUp();
        }

        public void Abort()
        {
            Aborted.Invoke();
            CleanUp();
        }

        public void SetAmount(int amount)
        {
            Amount.Value = amount;
            RefreshTotalPrice();
        }

        public void SetHaggleLevel(HaggleLevel level)
        {
            HaggleLevel = level;

            _hagglePriceModifier.Update(level);
            RefreshTotalPrice();
        }

        public override string ToString()
        {
            return
                $"{Good}x{Amount.Value} for a total of {TotalPrice.Value:0.##} at {HaggleLevel} ({ReputationChange.Value:0.#} rep)";
        }

        private void RefreshTotalPrice()
        {
            TotalPrice.Value = Amount.Value * SinglePrice.Value;
            RefreshProfit();
            RefreshReputationChange();
        }

        private void RefreshProfit()
        {
            var trackedInfo = _tradeTracker.TrackedGoods.GetValueOrDefault(Good);
            if (trackedInfo == null)
            {
                Debug.LogWarning($"TradeTracker did not have entry for {Good}. Something's wrong.");
                Profit.Value = null;
                return;
            }

            if (Amount <= 0)
            {
                Profit.Value = null;
                return;
            }

            Profit.Value = TotalPrice - trackedInfo.AveragePrice * Amount;
        }

        private void RefreshReputationChange()
        {
            var goodAmountFactor = (float)Amount.Value / 100;
            var repChangePer100Resources = _haggleConfig.Configs[HaggleLevel].ReputationPer100Goods;
            var finalRepChange = goodAmountFactor * repChangePer100Resources;
            ReputationChange.Value = finalRepChange;
        }
    }
}