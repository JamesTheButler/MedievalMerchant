using System.Collections;
using UnityEngine;

namespace Features.Tutorial.Onboarding.Logic.Steps
{
    public sealed class OnboardingExplainerStep : IOnboardingStep
    {
        public OnboardingTask Task => null;

        public void Initialize() { }

        public IEnumerator Run(OnboardingController controller)
        {
            var wasConfirmed = false;
            controller.PostExplainer(() => wasConfirmed = true);
            yield return new WaitUntil(() => wasConfirmed);
            controller.HideExplainer();
        }

        public void CleanUp() { }
    }
}