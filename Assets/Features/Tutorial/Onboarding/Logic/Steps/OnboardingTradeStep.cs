using System.Collections;
using Common.Infrastructure.Gameplay;
using Common.Types;
using Features.Towns;
using Features.Trade;
using Features.Trade.Logic;
using UnityEngine;

namespace Features.Tutorial.Onboarding.Logic.Steps
{
    public sealed class OnboardingTradeStep : IOnboardingStep
    {
        public OnboardingTask Task { get; }
        private readonly TradeType _tradeType;
        private readonly Good _good;
        private readonly int _amount;
        private readonly Town _town;

        private const string TradeButtonName = "Trade Button";
        private const string SliderName = "AmountSlider";
        
        private int _tradedAmount;

        private TradeService _tradeService;

        public OnboardingTradeStep(TradeType tradeType, Good good, int amount, Town town, OnboardingTask task = null)
        {
            _tradeType = tradeType;
            _good = good;
            _amount = amount;
            _town = town;
            Task = task;
        }

        public void Initialize()
        {
            _tradeService = GameplayContext.Instance.Services.TradeService;
            _tradeService.TradeCompleted.Observe(OnTradeCompleted);
        }

        public IEnumerator Run(OnboardingController controller)
        {
            controller.Blink(_town);
            yield return new WaitUntil(() => controller.TradeUI.IsOpen);
            controller.Blink(controller.TradeUI.transform.Find("Trade Button") as RectTransform);
            // highlight sequence:
            // - click _town
            // - for buy 
                // - click town producer.where(producer.good == _good)
                // - click slider where the value would be
                // - click buy button
                
            
            //if (_tradeType == TradeType.Buy)
            //{
            //    controller.BlinkTownProducerCell(_good);
            //}
            //else
            //{
            //    controller.BlinkPlayerInventoryCell(_good);
            //}

            yield return new WaitUntil(() => _tradedAmount >= _amount);

            controller.HideBlinker();
        }

        private void OnTradeCompleted(CompletedTrade trade)
        {
            if (trade.Good != _good || trade.TradeType != _tradeType || trade.Town != _town)
                return;

            _tradedAmount += trade.Amount;
        }

        public void CleanUp()
        {
            _tradeService.TradeCompleted.StopObserving(OnTradeCompleted);
        }
    }
}