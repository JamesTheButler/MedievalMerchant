using System.Collections;
using Common.Infrastructure.Observation;
using UnityEngine;

namespace Features.Tutorial.Onboarding.Logic
{
    public sealed class OnboardingExplainerStep : IOnboardingStep
    {
        private readonly int _messageId;

        public ObservableEvent Completed { get; } = new();

        public OnboardingExplainerStep(int messageId)
        {
            _messageId = messageId;
        }

        public void Initialize() { }

        public IEnumerator Run(OnboardingContext context)
        {
            var wasConfirmed = false;
            context.PostExplainer(_messageId, () => wasConfirmed = true);
            yield return new WaitUntil(() => wasConfirmed);
            context.HideExplainer();
        }

        public void CleanUp() { }
    }
}