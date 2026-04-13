using Common.Infrastructure.Gameplay;
using Common.UI.Elements;
using UnityEngine;

namespace Features.Levels.FeatureFlags
{
    public sealed class FeatureFlagObjectToggler : InitializableBehavior
    {
        private enum FlagCondition
        {
            IsActive,
            IsNotActive
        }

        private enum ObjectAction
        {
            Enable,
            Disable
        }

        [SerializeField]
        private LevelFeatureFlags flags;

        [SerializeField]
        private FlagCondition condition = FlagCondition.IsActive;

        [SerializeField]
        private ObjectAction action = ObjectAction.Enable;

        public override void Initialize()
        {
            var hasFlag = GameplayContext.Instance.LevelInfo.HasFeature(flags);
            var conditionMet = condition == FlagCondition.IsActive ? hasFlag : !hasFlag;
            gameObject.SetActive(action == ObjectAction.Enable ? conditionMet : !conditionMet);
        }
    }
}