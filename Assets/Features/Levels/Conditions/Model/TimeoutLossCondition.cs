using Common.Infrastructure.Observation;
using Common.Types;
using Features.Levels.Conditions.Data;
using Features.Levels.Conditions.Logic;

namespace Features.Levels.Conditions.Model
{
    public sealed class TimeoutLossCondition : ILossCondition
    {
        private readonly TimeoutLossConditionData _data;

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
            _data = data;
            DeadlineDate = new Date(1, data.DeadlineYear);
            WarningThresholdDaysLeft = data.DaysLeftWarning;

            Progress = new Progress(DeadlineDate.AsDays(), FormatProgress);
        }

        private static string FormatProgress(int currentValue, int maxValue)
        {
            var daysLeft = maxValue - currentValue;
            return $"{daysLeft} days left";
        }
    }
}