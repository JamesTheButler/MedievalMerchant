using System.Collections;
using Common.Infrastructure.Gameplay;
using Common.Types;
using Features.Trade.Logic;

namespace Features.Tutorial.Onboarding.Logic
{
    public sealed class OnboardingPurchaseStep : IOnboardingStep
    {
        private readonly Good _good;
        private readonly int _amount;

        private int _tradedAmount;

        private TradeService _tradeService;

        public OnboardingPurchaseStep(Good good, int amount)
        {
            _good = good;
            _amount = amount;
        }

        public IEnumerator Run(OnboardingContext context)
        {
            //context.PostTask();
            //context.
            //
            yield return null;
        }

        public void Initialize()
        {
            _tradeService = GameplayContext.Instance.Services.TradeService;
            _tradeService.TradeCompleted.Observe(OnTradeCompleted);
        }

        private void OnTradeCompleted(CompletedTrade trade)
        {
            if (trade.Good != _good)
                return;

            _tradedAmount += trade.Amount;
        }

        public void CleanUp() { }
    }
}