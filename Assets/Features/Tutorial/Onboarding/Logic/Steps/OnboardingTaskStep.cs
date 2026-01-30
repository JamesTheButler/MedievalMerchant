using System.Collections;
using System.Collections.Generic;

namespace Features.Tutorial.Onboarding.Logic.Steps
{
    public sealed class OnboardingTaskStep : IOnboardingStep
    {
        private readonly List<OnboardingTask> _tasks;

        public OnboardingTask Task { get; }

        public OnboardingTaskStep(List<OnboardingTask> tasks, OnboardingTask task = null)
        {
            _tasks = tasks;
            Task = task;
        }

        public void Initialize() { }

        public IEnumerator Run(OnboardingController controller)
        {
            controller.AddTasks(_tasks);
            yield return null;
        }

        public void CleanUp() { }
    }
}