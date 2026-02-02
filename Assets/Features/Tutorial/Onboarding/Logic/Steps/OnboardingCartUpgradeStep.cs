using System.Collections;
using System.Linq;
using Common.Infrastructure.Gameplay;
using Features.Player.Caravan.Logic;
using UnityEngine;

namespace Features.Tutorial.Onboarding.Logic.Steps
{
    public sealed class OnboardingCartUpgradeStep : IOnboardingStep
    {
        public OnboardingTask Task { get; }

        private readonly int _level;

        private Cart _cart1;

        public OnboardingCartUpgradeStep(int level, OnboardingTask task)
        {
            Task = task;
            _level = level;
        }

        public void Initialize()
        {
            _cart1 = GameplayContext.Instance.Model.Player.CaravanManager.Carts.First();
        }

        public IEnumerator Run(OnboardingController controller)
        {
            controller.Blink(controller.CaravanPanelUI.GetUpgradeButton(0));
            yield return new WaitUntil(() => _cart1.Level == _level);
            controller.HideBlinker();
        }

        public void CleanUp() { }
    }
}