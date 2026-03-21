using Common.Infrastructure;
using Common.Types;
using Common.Utility;
using Features.Levels.Conditions.Data;
using Features.Levels.Conditions.Logic;
using Features.Localization.Data;

namespace Features.Levels.Conditions.Model
{
    public sealed class TownTierWinCondition : IWinCondition
    {
        public Progress Progress { get; }

        public Tier TargetTier => _data.TargetTier;
        public int TargetCount => _data.TargetCount;
        public ConditionType Type => _data.Type;
        public string Description => _data.Description;

        private readonly ConditionsLocalizationResources _loc;
        private readonly TownTierWinConditionData _data;

        public TownTierWinCondition(TownTierWinConditionData data)
        {
            _loc = ResourceManager.Instance.LocalizationResources.Conditions;
            _data = data;

            Progress = new Progress(TargetCount, FormatProgress);
        }

        private string FormatProgress(int currentValue, int maxValue)
        {
            return _loc.TownTierProgress(currentValue, maxValue, TargetTier);
        }
    }
}