using System.Collections;
using Common.Infrastructure.Gameplay;
using Features.Ticking.Logic;
using UnityEngine;

namespace Features.Tutorial.Onboarding.Logic.Steps
{
    public sealed class OnboardingSetGameSpeedTask : IOnboardingStep
    {
        public OnboardingTask Task { get; }

        private GameSpeedModel _gameSpeedModel;

        public OnboardingSetGameSpeedTask(OnboardingTask task)
        {
            Task = task;
        }

        public void Initialize()
        {
            _gameSpeedModel = GameplayContext.Instance.Model.GameSpeed;
        }

        public IEnumerator Run(OnboardingController controller)
        {
            yield return new WaitUntil(() => _gameSpeedModel.GameSpeed.Value == GameSpeed.Fast);
        }

        public void CleanUp() { }
    }
}