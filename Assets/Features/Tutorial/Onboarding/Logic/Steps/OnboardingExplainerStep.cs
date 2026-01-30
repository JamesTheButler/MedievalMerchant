using System.Collections;
using UnityEngine;

namespace Features.Tutorial.Onboarding.Logic.Steps
{
    public sealed class OnboardingExplainerStep : IOnboardingStep
    {
        private readonly int _messageIndex;

        public OnboardingTask Task => null;

        public OnboardingExplainerStep(int messageIndex)
        {
            _messageIndex = messageIndex;
        }

        public void Initialize() { }

        public IEnumerator Run(OnboardingController controller)
        {
            var wasConfirmed = false;
            controller.PostExplainer(_messageIndex, () => wasConfirmed = true);
            yield return new WaitUntil(() => wasConfirmed);
            controller.HideExplainer();
        }

        public void CleanUp() { }
    }
}