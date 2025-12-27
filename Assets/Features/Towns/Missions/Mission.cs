using System;
using Common.Infrastructure.Observation;
using Common.Types;
using Features.Towns.Missions.Results;

namespace Features.Towns.Missions
{
    public sealed class Mission
    {
        public event Action<IMissionResult> MissionFailed, MissionSucceeded;

        public Good Good { get; }
        public int TotalCount { get; }
        public Date EndDate { get; }

        public IMissionResult Reward { get; }
        public IMissionResult Penalty { get; }

        public Observable<int> RemainingCount { get; } = new();

        public bool IsActive { get; private set; } = true;
        public bool IsSucceeded => RemainingCount.Value <= 0;

        public Mission(
            Good good,
            int totalCount,
            Date endDate,
            IMissionResult reward,
            IMissionResult penalty)
        {
            Good = good;
            TotalCount = totalCount;
            EndDate = endDate;
            Reward = reward;
            Penalty = penalty;
        }

        public void Deliver(int count)
        {
            if (!IsActive)
                return;

            RemainingCount.Value -= Math.Clamp(count, 0, RemainingCount);
            if (IsSucceeded)
            {
                IsActive = false;
                MissionSucceeded?.Invoke(Reward);
            }
        }

        public void Fail()
        {
            if (!IsActive)
                return;

            IsActive = false;
            MissionFailed?.Invoke(Penalty);
        }
    }
}