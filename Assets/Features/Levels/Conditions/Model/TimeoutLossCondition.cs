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

        public string WarningMessage =>
            $"You're running out of time! You have {WarningThresholdDaysLeft} days left to win.";

        public string GameOverMessage =>
            $"You've run out of time! You had until {DeadlineDate.ToDisplayString()} to win.";

        public ConditionType Type => _data.Type;
        public string Description => _data.Description;

        public TimeoutLossCondition(TimeoutLossConditionData data)
        {
            _data = data;
            DeadlineDate = new Date(data.DeadlineDay, data.DeadlineYear);
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