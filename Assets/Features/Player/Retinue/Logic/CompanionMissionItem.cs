using Common.Infrastructure.Observation;
using Common.Types;
using UnityEngine;

namespace Features.Player.Retinue.Logic
{
    public sealed class CompanionMissionItem
    {
        public Good Good { get; }
        public int TargetAmount { get; }

        public IReadOnlyObservable<int> RemainingAmount => _remainingAmount;

        public Observable<bool> IsCompleted { get; } = new();

        private readonly Observable<int> _remainingAmount;

        public CompanionMissionItem(Good good, int targetAmount)
        {
            Good = good;
            TargetAmount = targetAmount;
            _remainingAmount = new Observable<int>(targetAmount);
        }

        public void Deliver(int amount)
        {
            if (IsCompleted)
            {
                Debug.LogWarning($"Companion mission '{TargetAmount}x{Good}' is already completed. Skipping delivery.");
                return;
            }

            _remainingAmount.Value = Mathf.Max(0, TargetAmount - amount);
            if (_remainingAmount.Value > 0)
                return;

            IsCompleted.Value = true;
        }
    }
}