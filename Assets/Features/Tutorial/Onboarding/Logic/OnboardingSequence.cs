using System.Collections;
using Features.Tutorial.Onboarding.Logic.Steps;
using UnityEngine;

namespace Features.Tutorial.Onboarding.Logic
{
    public sealed class OnboardingSequence : IOnboardingStep
    {
        public OnboardingTask Task => null;

        private readonly float _delayBetweenSteps;
        private readonly IOnboardingStep[] _steps;

        public OnboardingSequence(float delayBetweenSteps, params IOnboardingStep[] steps)
        {
            _delayBetweenSteps = delayBetweenSteps;
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
                yield return new WaitForSeconds(_delayBetweenSteps);
            }
        }

        public void CleanUp() { }
    }
}