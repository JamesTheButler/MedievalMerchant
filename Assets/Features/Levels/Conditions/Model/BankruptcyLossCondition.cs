using Common.Infrastructure.Observation;
using Features.Levels.Conditions.Data;
using Features.Levels.Conditions.Logic;
using UnityEngine;

namespace Features.Levels.Conditions.Model
{
    public sealed class BankruptcyLossCondition : ILossCondition
    {
        private readonly BankruptcyLossConditionData _data;

        public Progress Progress { get; }
        public Observable<bool> IsClose { get; } = new();
        public int DaysLeftThreshold { get; }

        public string WarningMessage =>
            $"Your coffers are running dry. You have {DaysLeftThreshold} days to put coin back in your purse.";

        public string GameOverMessage =>
            $"You've run out of coin! You were bankrupt for more than {MaxBankruptcyDurationInDays} days.";

        public int BankruptcyFundsThreshold => _data.BankruptcyFundsThreshold;
        public int MaxBankruptcyDurationInDays => _data.MaxBankruptcyDurationInDays;
        public string Description => _data.Description;
        public ConditionType Type => _data.Type;

        public BankruptcyLossCondition(BankruptcyLossConditionData data)
        {
            _data = data;
            DaysLeftThreshold = _data.DaysLeftThreshold;

            Progress = new Progress(MaxBankruptcyDurationInDays, FormatProgress);
        }

        private static string FormatProgress(int currentValue, int maxValue)
        {
            return $"{maxValue - currentValue} days left in bankruptcy";
        }
    }
}