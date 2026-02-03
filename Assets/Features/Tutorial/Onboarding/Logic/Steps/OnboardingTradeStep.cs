using System.Collections;
using Common.Infrastructure.Gameplay;
using Common.Types;
using Common.UI.Elements;
using Features.Towns;
using Features.Trade;
using Features.Trade.Logic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

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
            while (_tradedAmount < _amount)
            {
                controller.Blink(_town, MouseButton.Left);

                yield return new WaitUntil(() => controller.TownUI.IsOpen);

                while (true)
                {
                    if (!controller.TownUI.IsOpen)
                        break;

                    // wait so that ui can be fully inflated (otherwise cell size is 0,0)
                    yield return new WaitForEndOfFrame();

                    GoodCell cell = _tradeType == TradeType.Buy
                        ? controller.TownProducerUI.GetCell(_good)
                        : controller.CaravanPanelUI.GetCell(_good);

                    if (!cell)
                    {
                        Debug.LogError($"Could not find cell for good '{_good}'.");
                        continue;
                    }

                    controller.Blink(cell, MouseButton.Left);

                    yield return new WaitUntil(() => controller.TradeUI.IsOpen);

                    break;
                }

                if (!controller.TradeUI.IsOpen)
                    continue;

                controller.Blink(controller.TradeUI.TradeButton, MouseButton.Left);

                yield return new WaitUntil(() => !controller.TradeUI.IsOpen);
            }


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