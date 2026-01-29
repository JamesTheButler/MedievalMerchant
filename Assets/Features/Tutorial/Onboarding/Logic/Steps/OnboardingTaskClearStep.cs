using System.Collections;

namespace Features.Tutorial.Onboarding.Logic.Steps
{
    public sealed class OnboardingTaskClearStep : IOnboardingStep
    {
        public void Initialize() { }

        public IEnumerator Run(OnboardingController controller)
        {
            controller.ClearTasks();
            yield return null;
        }

        public void CleanUp() { }
    }
}