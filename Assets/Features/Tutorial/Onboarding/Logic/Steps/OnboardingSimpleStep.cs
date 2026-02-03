using System;
using System.Collections;

namespace Features.Tutorial.Onboarding.Logic.Steps
{
    public sealed class OnboardingSimpleStep : IOnboardingStep
    {
        private readonly Action _action;

        public OnboardingTask Task => null;

        public OnboardingSimpleStep(Action action)
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