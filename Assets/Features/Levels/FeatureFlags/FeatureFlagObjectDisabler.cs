using Common.Infrastructure.Gameplay;
using Common.UI.Elements;
using UnityEngine;

namespace Features.Levels.FeatureFlags
{
    public sealed class FeatureFlagObjectDisabler : InitializableBehavior
    {
        [SerializeField]
        private LevelFeatureFlags disableIf;

        public override void Initialize()
        {
            var hasFlag = GameplayContext.Instance.LevelInfo.HasFeature(disableIf);
            gameObject.SetActive(!hasFlag);
        }
    }
}