using System;
using System.Collections;

namespace Features.Tutorial.Onboarding.Logic.Steps
{
    public sealed class SimpleOnboardingStep : IOnboardingStep
    {
        private readonly Action _action;

        public OnboardingTask Task => null;

        public SimpleOnboardingStep(Action action)
        {
            _action = action;
        }

        public void Initialize() { }

        public IEnumerator Run(OnboardingController controller)
        {
            _action?.Invoke();
            yield return null;
        }

        public void CleanUp() { }
    }
}