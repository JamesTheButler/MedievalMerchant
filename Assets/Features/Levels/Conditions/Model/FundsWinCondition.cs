using Features.Levels.Conditions.Data;
using Features.Levels.Conditions.Logic;

namespace Features.Levels.Conditions.Model
{
    public sealed class FundsWinCondition : IWinCondition
    {
        private readonly FundsWinConditionData _data;

        public Progress Progress { get; }

        public ConditionType Type => _data.Type;
        public int FundsToReach => _data.FundsToReach;
        public string Description => _data.Description;

        public FundsWinCondition(FundsWinConditionData data)
        {
            _data = data;
            Progress = new Progress(FundsToReach, FormatProgress);
        }

        private static string FormatProgress(int currentValue, int maxValue)
        {
            return $"{currentValue}/{maxValue} coin";
        }
    }
}