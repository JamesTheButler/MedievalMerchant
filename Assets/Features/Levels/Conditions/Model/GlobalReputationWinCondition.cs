using Common.Infrastructure;
using Features.Levels.Conditions.Data;
using Features.Levels.Conditions.Logic;
using Features.Localization.Data;

namespace Features.Levels.Conditions.Model
{
    public sealed class GlobalReputationWinCondition : IWinCondition
    {
        private readonly GlobalReputationWinConditionData _data;
        private readonly ConditionsLocalizationResources _loc;

        public Progress Progress { get; }

        public ConditionType Type => _data.Type;
        public int GlobalAverageReputationTarget => _data.GlobalAverageReputation;
        public string Description => _data.Description;

        public GlobalReputationWinCondition(GlobalReputationWinConditionData data)
        {
            _loc = ResourceManager.Instance.LocalizationResources.Conditions;
            _data = data;
            Progress = new Progress(GlobalAverageReputationTarget, FormatProgress);
        }

        private string FormatProgress(int currentValue, int maxValue)
        {
            return _loc.GlobalRepProgress(currentValue, maxValue);
        }
    }
}