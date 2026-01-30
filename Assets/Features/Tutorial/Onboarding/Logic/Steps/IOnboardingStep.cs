using System.Collections;
using Common.Infrastructure;

namespace Features.Tutorial.Onboarding.Logic.Steps
{
    public interface IOnboardingStep : IInitializable
    {
        public OnboardingTask Task { get; }
        IEnumerator Run(OnboardingController controller);
    }
}