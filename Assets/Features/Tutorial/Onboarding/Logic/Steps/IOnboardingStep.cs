using System.Collections;
using Common.Infrastructure;

namespace Features.Tutorial.Onboarding.Logic.Steps
{
    public interface IOnboardingStep : IInitializable
    {
        IEnumerator Run(OnboardingController controller);
    }
}