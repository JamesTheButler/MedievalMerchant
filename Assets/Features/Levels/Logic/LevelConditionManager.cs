using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Features.Levels.Config;
using Features.Levels.Config.Conditions;
using Infrastructure;

namespace Features.Levels.Logic
{
    public sealed class LevelConditionManager : ISystem
    {
        public IReadOnlyList<WinCondition> WinConditions => _winConditions;
        public IReadOnlyList<LossCondition> LossConditions => _lossConditions;

        public event Action LevelWon, LevelLost;
        public event Action<int> CompletionCountChanged;
        public Observable<bool> IsLossClose { get; } = new();

        private List<WinCondition> _winConditions = new();
        private List<LossCondition> _lossConditions = new();
        private ConditionConfig _conditionConfig;

        private readonly HashSet<LossCondition> _closeLossConditions = new();

        public void Initialize()
        {
            _conditionConfig = ConfigurationManager.Configurations.ConditionConfig;
            var conditions = GlobalContext.CurrentLevelInfo!.Conditions;
            _winConditions = conditions.OfType<WinCondition>().ToList();
            _lossConditions = conditions.OfType<LossCondition>().ToList();

            foreach (var condition in _winConditions)
            {
                condition.Initialize();
                condition.Progress.IsCompleted.Observe(_ => OnWinConditionChanged(condition), false);
            }

            foreach (var condition in _lossConditions)
            {
                condition.Initialize();
                condition.Progress.IsCompleted.Observe(OnLossConditionChanged, false);
                condition.Progress.CurrentValuePercent.Observe(
                    percent => OnLossConditionProgressChanged(percent, condition), false);
            }
        }

        public void CleanUp() { }

        private void OnLossConditionProgressChanged(float currentProgressPercent, LossCondition condition)
        {
            if (currentProgressPercent >= _conditionConfig.WarningThresholdPercent)
            {
                _closeLossConditions.Add(condition);
            }
            else
            {
                _closeLossConditions.Remove(condition);
            }

            var anyCloseLossConditions = _closeLossConditions.Any();
            IsLossClose.Value = anyCloseLossConditions;
        }

        private void OnWinConditionChanged(WinCondition _)
        {
            var count = _winConditions.Count(condition => condition.Progress.IsCompleted);
            CompletionCountChanged?.Invoke(count);

            if (_winConditions.Count == count)
            {
                LevelWon?.Invoke();
            }
        }

        private void OnLossConditionChanged(bool isCompleted)
        {
            if (isCompleted)
            {
                LevelLost?.Invoke();
            }
        }
    }
}