using Features.Levels.Conditions.Data;
using Features.Levels.Conditions.Logic;

namespace Features.Levels.Conditions.Model
{
    public sealed class GlobalReputationWinCondition : IWinCondition
    {
        private readonly GlobalReputationWinConditionData _data;

        public Progress Progress { get; }

        public ConditionType Type => _data.Type;
        public int GlobalAverageReputationTarget => _data.GlobalAverageReputation;
        public string Description => _data.Description;

        public GlobalReputationWinCondition(GlobalReputationWinConditionData data)
        {
            _data = data;
            Progress = new Progress(GlobalAverageReputationTarget, FormatProgress);
        }

        private static string FormatProgress(int currentValue, int maxValue)
        {
            return $"{currentValue}/{maxValue} global reputation";
        }
    }
}