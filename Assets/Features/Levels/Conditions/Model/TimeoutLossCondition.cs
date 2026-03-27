using Common.Infrastructure;
using Common.Infrastructure.Observation;
using Common.Types;
using Features.Levels.Conditions.Data;
using Features.Levels.Conditions.Logic;
using Features.Localization.Data;

namespace Features.Levels.Conditions.Model
{
    public sealed class TimeoutLossCondition : ILossCondition
    {
        private readonly TimeoutLossConditionData _data;
        private readonly ConditionsLocalizationResources _loc;

        public Progress Progress { get; }
        public Date DeadlineDate { get; }
        public Observable<bool> IsClose { get; } = new();

        public int WarningThresholdDaysLeft { get; }

        public ConditionType Type => _data.Type;
        public string Description => _data.Description;
        public string WarningMessage => _data.WarningMessage;
        public string GameOverMessage => _data.GameOverMessage;

        public TimeoutLossCondition(TimeoutLossConditionData data)
        {
            _loc = ResourceManager.Instance.LocalizationResources.Conditions;
            _data = data;
            DeadlineDate = new Date(DateModel.LastDayOfYear, data.DeadlineYear);
            WarningThresholdDaysLeft = data.DaysLeftWarning;

            Progress = new Progress(DeadlineDate.AsDays(), FormatProgress);
        }

        private string FormatProgress(int currentValue, int maxValue)
        {
            return _loc.TimeoutProgress(maxValue - currentValue);
        }
    }
}