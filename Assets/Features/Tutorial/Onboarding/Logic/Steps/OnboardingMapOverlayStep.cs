using System.Collections;
using Common.Infrastructure.Gameplay;
using Features.Map.Modes;
using UnityEngine;

namespace Features.Tutorial.Onboarding.Logic.Steps
{
    public sealed class OnboardingMapOverlayStep : IOnboardingStep
    {
        public OnboardingTask Task { get; }

        private MapModeModel _mapModeModel;

        private bool _isCompleted;

        public OnboardingMapOverlayStep(OnboardingTask task)
        {
            Task = task;
        }

        public void Initialize()
        {
            _mapModeModel = GameplayContext.Instance.Model.MapModeModel;
            _mapModeModel.MapMode.Observe(OnMapModeChanged);
        }

        private void OnMapModeChanged(MapMode obj)
        {
            if (obj != MapMode.Town)
                return;

            _isCompleted = true;
        }

        public IEnumerator Run(OnboardingController controller)
        {
            yield return new WaitUntil(() => _isCompleted);
        }

        public void CleanUp() { }
    }
}