using Common.Infrastructure.Gameplay;
using Common.UI.Elements;
using UnityEngine;

namespace Features.Levels.FeatureFlags
{
    public sealed class FeatureFlagObjectDestroyer : InitializableBehavior
    {
        [SerializeField]
        private LevelFeatureFlags keepIf;

        public override void Initialize()
        {
            if (GameplayContext.Instance.LevelInfo.HasFeature(keepIf))
                return;

            Destroy(gameObject);
        }
    }
}