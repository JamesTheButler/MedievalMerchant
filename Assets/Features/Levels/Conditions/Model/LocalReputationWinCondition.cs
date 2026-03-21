using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Features.Levels.Conditions.Data;
using Features.Levels.Conditions.Logic;
using Features.Localization.Data;

namespace Features.Levels.Conditions.Model
{
    public sealed class LocalReputationWinCondition : IWinCondition
    {
        private readonly LocalReputationWinConditionData _data;
        private readonly ConditionsLocalizationResources _loc;

        public Progress Progress { get; }

        public ConditionType Type => _data.Type;
        public int Reputation => _data.MinRepPerTown;
        public string Description => _data.Description;

        public LocalReputationWinCondition(LocalReputationWinConditionData data)
        {
            _loc = ResourceManager.Instance.LocalizationResources.Conditions;
            _data = data;
            var model = GameplayContext.Instance.Model;
            Progress = new Progress(model.Towns.Count, FormatProgress);
        }

        private string FormatProgress(int currentValue, int maxValue)
        {
            return _loc.LocalRepProgress(currentValue, maxValue);
        }
    }
}