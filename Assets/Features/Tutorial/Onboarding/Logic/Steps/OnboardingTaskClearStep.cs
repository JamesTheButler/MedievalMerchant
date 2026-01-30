using System.Collections;
using UnityEngine;

namespace Features.Tutorial.Onboarding.Logic.Steps
{
    public sealed class OnboardingTaskClearStep : IOnboardingStep
    {
        public void Initialize() { }

        public OnboardingTask Task { get; }

        public OnboardingTaskClearStep(OnboardingTask task = null)
        {
            Task = task;
        }

        public IEnumerator Run(OnboardingController controller)
        {
            yield return new WaitForSeconds(1.5f);
            controller.ClearTasks();
            yield return null;
        }

        public void CleanUp() { }
    }
}