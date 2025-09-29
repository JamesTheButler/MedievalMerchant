using System.Collections.Generic;
using System.Linq;
using Levels.Conditions;
using UnityEngine;
using UnityEngine.Events;

namespace Data
{
    public sealed class ConditionManager : MonoBehaviour
    {
        public IReadOnlyList<WinCondition> WinConditions => _winConditions;
        public IReadOnlyList<LossCondition> LossConditions => _lossConditions;

        [SerializeField]
        private UnityEvent levelWon;

        [SerializeField]
        private UnityEvent levelLost;

        private List<WinCondition> _winConditions = new();
        private List<LossCondition> _lossConditions = new();

        public void Setup(IEnumerable<Condition> conditions)
        {
            var enumeratedConditions = conditions as Condition[] ?? conditions.ToArray();
            _winConditions = enumeratedConditions.OfType<WinCondition>().ToList();
            _lossConditions = enumeratedConditions.OfType<LossCondition>().ToList();

            foreach (var condition in _winConditions)
            {
                condition.Initialize();
                condition.IsCompleted.Observe(_ => OnWinConditionChanged(condition), false);
            }

            foreach (var condition in _lossConditions)
            {
                condition.Initialize();
                condition.IsCompleted.Observe(OnLossConditionChanged, false);
            }
        }

        private void OnWinConditionChanged(WinCondition _)
        {
            if (_winConditions.All(condition => condition.IsCompleted))
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