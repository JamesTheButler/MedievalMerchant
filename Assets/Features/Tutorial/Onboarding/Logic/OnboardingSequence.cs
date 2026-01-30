using System.Collections;
using Features.Tutorial.Onboarding.Logic.Steps;
using UnityEngine;

namespace Features.Tutorial.Onboarding.Logic
{
    public sealed class OnboardingSequence : IOnboardingStep
    {
        public OnboardingTask Task => null;

        private readonly IOnboardingStep[] _steps;

        public OnboardingSequence(params IOnboardingStep[] steps)
        {
            _steps = steps;
        }

        public void Initialize() { }

        public IEnumerator Run(OnboardingController controller)
        {
            foreach (var step in _steps)
            {
                step.Initialize();
                yield return step.Run(controller);
                step.CleanUp();
                step.Task?.Complete();
                yield return new WaitForSeconds(1f);
            }
        }

        public void CleanUp() { }
    }
}