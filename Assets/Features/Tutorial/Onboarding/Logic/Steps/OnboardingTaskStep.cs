using System.Collections;
using System.Collections.Generic;

namespace Features.Tutorial.Onboarding.Logic.Steps
{
    public sealed class OnboardingTaskStep : IOnboardingStep
    {
        private readonly List<string> _tasks;

        public OnboardingTaskStep(List<string> tasks)
        {
            _tasks = tasks;
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