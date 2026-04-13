using Common.Infrastructure.Gameplay;
using Common.UI.Elements;
using UnityEngine;

namespace Features.Levels.FeatureFlags
{
    public sealed class FeatureFlagObjectEnabler : InitializableBehavior
    {
        [SerializeField]
        private LevelFeatureFlags enableIf;

        public override void Initialize()
        {
            var hasFlag = GameplayContext.Instance.LevelInfo.HasFeature(enableIf);
            gameObject.SetActive(hasFlag);
        }
    }
}