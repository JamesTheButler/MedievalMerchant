using Common.Infrastructure;
using Features.Levels.Conditions.Data;
using Features.Levels.Conditions.Logic;
using Features.Localization.Data;

namespace Features.Levels.Conditions.Model
{
    public sealed class FundsWinCondition : IWinCondition
    {
        private readonly FundsWinConditionData _data;
        private readonly ConditionsLocalizationResources _loc;

        public Progress Progress { get; }

        public ConditionType Type => _data.Type;
        public int FundsToReach => _data.FundsToReach;
        public string Description => _data.Description;

        public FundsWinCondition(FundsWinConditionData data)
        {
            _loc = ResourceManager.Instance.LocalizationResources.Conditions;
            _data = data;
            Progress = new Progress(FundsToReach, FormatProgress);
        }

        private string FormatProgress(int currentValue, int maxValue)
        {
            return _loc.FundsProgress(currentValue, maxValue);
        }
    }
}