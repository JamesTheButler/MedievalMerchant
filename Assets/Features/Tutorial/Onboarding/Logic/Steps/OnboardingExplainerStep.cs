using System.Collections;
using Features.Tutorial.Onboarding.Data;
using UnityEngine;

namespace Features.Tutorial.Onboarding.Logic.Steps
{
    public sealed class OnboardingExplainerStep : IOnboardingStep
    {
        private readonly OnboardingExplainer _explainer;

        public OnboardingTask Task => null;

        public OnboardingExplainerStep(OnboardingExplainer explainer)
        {
            _explainer = explainer;
        }

        public void Initialize() { }

        public IEnumerator Run(OnboardingController controller)
        {
            var wasConfirmed = false;
            controller.PostExplainer(_explainer, () => wasConfirmed = true);
            yield return new WaitUntil(() => wasConfirmed);
            controller.HideExplainer();
        }

        public void CleanUp() { }
    }
}