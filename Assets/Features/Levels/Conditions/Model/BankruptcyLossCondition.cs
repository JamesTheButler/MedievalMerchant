using Features.Levels.Conditions.Data;
using Features.Levels.Conditions.Logic;

namespace Features.Levels.Conditions.Model
{
    public sealed class BankruptcyLossCondition : ILossCondition
    {
        private readonly BankruptcyLossConditionData _data;

        public Progress Progress { get; }
        public string GameOverMessage => "You've run out of money!";

        public int BankruptcyFundsThreshold => _data.BankruptcyFundsThreshold;
        public int MaxBankruptcyDurationInDays => _data.MaxBankruptcyDurationInDays;
        public string Description => _data.Description;
        public ConditionType Type => _data.Type;

        public BankruptcyLossCondition(BankruptcyLossConditionData data)
        {
            _data = data;

            Progress = new Progress(MaxBankruptcyDurationInDays, FormatProgress);
        }

        private static string FormatProgress(int currentValue, int maxValue)
        {
            return $"{maxValue - currentValue} days left in bankruptcy";
        }
    }
}