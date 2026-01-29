using Common.Infrastructure.Global;
using Common.UI.Elements;

namespace Features.Tutorial.UI
{
    public sealed class TutorialObjectDestroyer : InitializableBehavior
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