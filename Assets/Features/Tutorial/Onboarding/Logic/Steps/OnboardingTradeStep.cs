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
            if (_tradeType == TradeType.Buy)
            {
                while (_tradedAmount < _amount)
                {
                    controller.Blink(_town);

                    yield return new WaitUntil(() => controller.TownUI.IsOpen);

                    while (true)
                    {
                        if (!controller.TownUI.IsOpen)
                            break;

                        // wait so that ui can be fully inflated (otherwise cell size is 0,0)
                        yield return new WaitForEndOfFrame();

                        var cell = controller.TownProducerUI.GetCell(_good);
                        if (!cell)
                        {
                            Debug.LogError($"Could not find cell for good '{_good}'.");
                            continue;
                        }

                        controller.Blink(cell);

                        yield return new WaitUntil(() => controller.TradeUI.IsOpen);

                        break;
                    }

                    if (!controller.TradeUI.IsOpen)
                        continue;

                    controller.Blink(controller.TradeUI.TradeButton);

                    yield return new WaitUntil(() => !controller.TradeUI.IsOpen);
                }
            }
            else
            {
                while (_tradedAmount < _amount)
                {
                    yield return SellBlinkSequence(controller);
                }
            }

            controller.HideBlinker();
        }

        private IEnumerator BuyBlinkSequence(OnboardingController controller)
        {
            controller.Blink(_town);
            yield return new WaitUntil(() => controller.TownUI.IsOpen);
            var cell = controller.TownProducerUI.GetCell(_good);
            if (!cell)
            {
                Debug.LogError($"Could not find cell for good '{_good}'.");
                yield return null;
            }

            controller.Blink(cell);

            yield return new WaitUntil(() => controller.TradeUI.IsOpen);
            controller.Blink(controller.TradeUI.TradeButton);
        }

        private IEnumerator SellBlinkSequence(OnboardingController controller)
        {
            // for sell
            // - click _town
            // - click caravan panel ui - goodcell
            // - click "15/30" button
            // 
            yield return null;
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