using System.Collections.Generic;
using System.Linq;
using Common.Infrastructure.Observation;
using Features.Levels.Conditions.Data;

namespace Features.Levels.Conditions.Model
{
    public sealed class LevelConditions
    {
        public IReadOnlyList<IWinCondition> WinConditions => _winConditions;
        public IReadOnlyList<ILossCondition> LossConditions => _lossConditions;

        public ObservableEvent LevelWon { get; } = new();
        public ObservableEvent<ILossCondition> LevelLost { get; } = new();
        public ObservableEvent<int> CompletionCountChanged { get; } = new();
        public Observable<bool> IsLossClose { get; } = new();

        private readonly ConditionModelFactory _factory = new();

        private readonly List<IWinCondition> _winConditions = new();
        private readonly List<ILossCondition> _lossConditions = new();
        private readonly HashSet<ILossCondition> _closeLossConditions = new();

        public void Initialize(IEnumerable<ConditionData> conditions)
        {
            PopulateConditions(conditions);
        }

        private void PopulateConditions(IEnumerable<ConditionData> conditionDatas)
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
                        lossCondition.Progress.IsCompleted.Observe(
                            isCompleted => OnLossConditionChanged(lossCondition, isCompleted), false);
                        lossCondition.IsClose.Observe(isClose => OnIsCloseChanged(lossCondition, isClose), false);
                        break;
                }
            }
        }

        private void OnIsCloseChanged(ILossCondition lossCondition, bool isClose)
        {
            if (isClose)
            {
                _closeLossConditions.Add(lossCondition);
            }
            else
            {
                _closeLossConditions.Remove(lossCondition);
            }

            IsLossClose.Value = _closeLossConditions.Any();
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

        private void OnLossConditionChanged(ILossCondition lossCondition, bool isCompleted)
        {
            if (!isCompleted)
                return;

            LevelLost?.Invoke(lossCondition);
        }
    }
}