using System.Collections;
using Common.Infrastructure.Gameplay;
using Common.Types;
using Features.Trade;
using Features.Trade.Logic;
using UnityEngine;

namespace Features.Tutorial.Onboarding.Logic.Steps
{
    public sealed class OnboardingTradeStep : IOnboardingStep
    {
        private readonly TradeType _tradeType;
        private readonly Good _good;
        private readonly int _amount;

        private int _tradedAmount;

        private TradeService _tradeService;

        public OnboardingTradeStep(TradeType tradeType, Good good, int amount)
        {
            _tradeType = tradeType;
            _good = good;
            _amount = amount;
        }

        public void Initialize()
        {
            _tradeService = GameplayContext.Instance.Services.TradeService;
            _tradeService.TradeCompleted.Observe(OnTradeCompleted);
        }

        public IEnumerator Run(OnboardingController controller)
        {
            if (_tradeType == TradeType.Buy)
            {
                controller.BlinkTownProducerCell(_good);
            }
            else
            {
                controller.BlinkPlayerInventoryCell(_good);
            }

            yield return new WaitUntil(() => _tradedAmount >= _amount);

            controller.HideBlinker();
        }

        private void OnTradeCompleted(CompletedTrade trade)
        {
            if (trade.Good != _good || trade.TradeType != _tradeType)
                return;

            _tradedAmount += trade.Amount;
        }

        public void CleanUp()
        {
            _tradeService.TradeCompleted.StopObserving(OnTradeCompleted);
        }
    }
}