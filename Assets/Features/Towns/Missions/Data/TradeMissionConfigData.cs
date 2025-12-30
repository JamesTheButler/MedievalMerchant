using System;
using Features.Towns.Missions.Results;
using UnityEngine;

namespace Features.Towns.Missions.Data
{
    [Serializable]
    public sealed class TradeMissionConfigData
    {
        [field: SerializeField, Range(0, 3)]
        public int MaxMissionCount { get; private set; } = 1;

        [field: SerializeField, Range(0f, 1f)]
        public float DailyMissionChance { get; private set; } = 0.05f;

        [field: SerializeField]
        public int LengthInDays { get; private set; } = 30;

        [field: SerializeField]
        public int Volume { get; private set; } = 15;

        [field: SerializeField]
        public float GoldReward { get; private set; } = 500f;

        [field: SerializeField, Range(0, 100)]
        public float ReputationReward { get; private set; } = 5f;

        [field: SerializeField, Range(0, 100)]
        public float GrowthReward { get; private set; } = 10f;

        [field: SerializeField, Range(-100, 0)]
        public float ReputationPenalty { get; private set; } = -7.5f;

        [field: SerializeField, Range(-100, 0)]
        public float GrowthPenalty { get; private set; } = -12.5f;

        public IMissionResult GetReward()
        {
            return new TradeMissionReward(GoldReward, ReputationReward, GrowthReward);
        }

        public IMissionResult GetPenalty()
        {
            return new TradeMissionPenalty(ReputationPenalty, GrowthPenalty);
        }
    }
}