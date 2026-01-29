using System.Collections;
using Common.Infrastructure;

namespace Features.Tutorial.Onboarding.Logic
{
    public interface IOnboardingStep : IInitializable
    {
        IEnumerator Run(OnboardingContext context);
    }
}