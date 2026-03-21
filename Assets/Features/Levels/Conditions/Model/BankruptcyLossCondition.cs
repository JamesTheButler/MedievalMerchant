using Common.Infrastructure;
using Common.Infrastructure.Observation;
using Features.Levels.Conditions.Data;
using Features.Levels.Conditions.Logic;
using Features.Localization.Data;

namespace Features.Levels.Conditions.Model
{
    public sealed class BankruptcyLossCondition : ILossCondition
    {
        private readonly BankruptcyLossConditionData _data;
        private readonly ConditionsLocalizationResources _loc;

        public Progress Progress { get; }
        public Observable<bool> IsClose { get; } = new();
        public int DaysLeftThreshold { get; }

        public string WarningMessage => _data.WarningMessage;

        public string GameOverMessage => _data.GameOverMessage;

        public int BankruptcyFundsThreshold => _data.BankruptcyFundsThreshold;
        public int MaxBankruptcyDurationInDays => _data.MaxBankruptcyDurationInDays;
        public string Description => _data.Description;
        public ConditionType Type => _data.Type;

        public BankruptcyLossCondition(BankruptcyLossConditionData data)
        {
            _loc = ResourceManager.Instance.LocalizationResources.Conditions;
            _data = data;
            DaysLeftThreshold = _data.DaysLeftThreshold;

            Progress = new Progress(MaxBankruptcyDurationInDays, FormatProgress);
        }

        private string FormatProgress(int currentValue, int maxValue)
        {
            return _loc.TimeoutProgress(maxValue - currentValue);
        }
    }
}