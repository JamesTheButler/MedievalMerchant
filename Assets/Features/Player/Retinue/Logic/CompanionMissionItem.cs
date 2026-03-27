using Common.Infrastructure.Observation;
using UnityEngine;

namespace Features.Player.Retinue.Logic
{
    public sealed class CompanionMissionItem
    {
        public int TargetAmount { get; }

        public IReadOnlyObservable<int> RemainingAmount => _remainingAmount;

        public Observable<bool> IsCompleted { get; } = new();

        private readonly Observable<int> _remainingAmount;

        public CompanionMissionItem(int targetAmount)
        {
            TargetAmount = targetAmount;
            _remainingAmount = new Observable<int>(targetAmount);
        }

        public void Deliver(int amount)
        {
            if (IsCompleted)
            {
                Debug.LogWarning($"Companion mission is already completed. Skipping delivery.");
                return;
            }

            _remainingAmount.Value = Mathf.Max(0, _remainingAmount.Value - amount);
            if (_remainingAmount.Value > 0)
                return;

            IsCompleted.Value = true;
        }
    }
}