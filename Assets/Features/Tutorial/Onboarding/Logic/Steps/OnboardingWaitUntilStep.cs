using System;
using System.Collections;
using UnityEngine;

namespace Features.Tutorial.Onboarding.Logic.Steps
{
    public sealed class OnboardingWaitUntilStep : IOnboardingStep
    {
        private readonly Func<bool> _predicate;

        public OnboardingTask Task => null;

        public OnboardingWaitUntilStep(Func<bool> predicate)
        {
            _predicate = predicate;
        }

        public void Initialize() { }

        public IEnumerator Run(OnboardingController controller)
        {
            yield return new WaitUntil(_predicate.Invoke);
        }

        public void CleanUp() { }
    }
}