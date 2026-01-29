using Common.Infrastructure.Global;
using Common.UI.Elements;

namespace Features.Tutorial.Onboarding.UI
{
    public sealed class OnboardingObjectDestroyer : InitializableBehavior
    {
        public override void Initialize()
        {
            if (!GlobalContext.CurrentLevelInfo!.IsTutorial)
            {
                Destroy(gameObject);
            }
        }
    }
}