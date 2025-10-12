using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Features.Levels.Config;
using Features.Levels.Config.Conditions;
using UnityEngine;
using UnityEngine.Events;

namespace Features.Levels.Logic
{
    public sealed class ConditionManager : MonoBehaviour
    {
        public IReadOnlyList<WinCondition> WinConditions => _winConditions;
        public IReadOnlyList<LossCondition> LossConditions => _lossConditions;

        [SerializeField]
        private UnityEvent levelWon;

        [SerializeField]
        private UnityEvent levelLost;

        public event Action<int> CompletionCountChanged;

        public Observable<bool> IsLossClose { get; } = new();

        private List<WinCondition> _winConditions = new();
        private List<LossCondition> _lossConditions = new();
        private ConditionConfig _conditionManager;

        private readonly HashSet<LossCondition> _closeLossConditions = new();
        

        private void Awake()
        {
            _conditionManager = ConfigurationManager.Instance.ConditionConfig;
        }

        public void Setup(IEnumerable<Condition> conditions)
        {
            var enumeratedConditions = conditions as Condition[] ?? conditions.ToArray();
            _winConditions = enumeratedConditions.OfType<WinCondition>().ToList();
            _lossConditions = enumeratedConditions.OfType<LossCondition>().ToList();

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

        private void OnLossConditionProgressChanged(float currentProgressPercent, LossCondition condition)
        {
            if (currentProgressPercent >= _conditionManager.WarningThresholdPercent)
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
                levelWon.Invoke();
            }
        }

        private void OnLossConditionChanged(bool isCompleted)
        {
            if (isCompleted)
            {
                levelLost.Invoke();
            }
        }
    }
}