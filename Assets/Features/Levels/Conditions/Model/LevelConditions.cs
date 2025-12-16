using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Features.Levels.Conditions.Data;
using Infrastructure;
using ConditionResources = Features.Levels.Conditions.Config.ConditionResources;

namespace Features.Levels.Conditions.Model
{
    public sealed class LevelConditions
    {
        public IReadOnlyList<IWinCondition> WinConditions => _winConditions;
        public IReadOnlyList<ILossCondition> LossConditions => _lossConditions;

        public event Action LevelWon, LevelLost;
        public event Action<int> CompletionCountChanged;
        public Observable<bool> IsLossClose { get; } = new();

        private readonly ConditionResources _conditionResources;
        private readonly ConditionModelFactory _factory = new();

        private readonly List<IWinCondition> _winConditions = new();
        private readonly List<ILossCondition> _lossConditions = new();
        private readonly HashSet<ILossCondition> _closeLossConditions = new();

        public LevelConditions()
        {
            _conditionResources = ResourceManager.Instance.ConditionResources;
            var conditionDatas = GlobalContext.CurrentLevelInfo!.Conditions;
            PopulateConditions(conditionDatas);
        }

        private void PopulateConditions(ConditionData[] conditionDatas)
        {
            var conditions = conditionDatas.Select(data => _factory.Get(data));
            foreach (var condition in conditions)
            {
                switch (condition)
                {
                    case IWinCondition winCondition:
                        _winConditions.Add(winCondition);
                        winCondition.Progress.IsCompleted.Observe(_ => OnWinConditionChanged(winCondition), false);
                        break;
                    case ILossCondition lossCondition:
                        _lossConditions.Add(lossCondition);
                        lossCondition.Progress.IsCompleted.Observe(OnLossConditionChanged, false);
                        lossCondition.Progress.CurrentValuePercent.Observe(
                            percent => OnLossConditionProgressChanged(percent, lossCondition), false);
                        break;
                }
            }
        }

        private void OnLossConditionProgressChanged(float currentProgressPercent, ILossCondition lossCondition)
        {
            if (currentProgressPercent >= _conditionResources.WarningThresholdPercent)
            {
                _closeLossConditions.Add(lossCondition);
            }
            else
            {
                _closeLossConditions.Remove(lossCondition);
            }

            var anyCloseLossConditions = _closeLossConditions.Any();
            IsLossClose.Value = anyCloseLossConditions;
        }

        private void OnWinConditionChanged(IWinCondition _)
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