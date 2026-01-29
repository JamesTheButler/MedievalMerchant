using System.Collections;
using UnityEngine;

namespace Features.Tutorial.Onboarding.Logic.Steps
{
    public sealed class OnboardingExplainerStep : IOnboardingStep
    {
        private readonly int _messageId;

        public OnboardingExplainerStep(int messageId)
        {
            _messageId = messageId;
        }

        public void Initialize() { }

        public IEnumerator Run(OnboardingController controller)
        {
            var wasConfirmed = false;
            controller.PostExplainer(_messageId, () => wasConfirmed = true);
            yield return new WaitUntil(() => wasConfirmed);
            controller.HideExplainer();
        }

        public void CleanUp() { }
    }
}