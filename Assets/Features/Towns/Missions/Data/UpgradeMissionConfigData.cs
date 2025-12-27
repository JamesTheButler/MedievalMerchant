using System;
using Features.Towns.Missions.Results;
using UnityEngine;

namespace Features.Towns.Missions.Data
{
    [Serializable]
    public sealed class UpgradeMissionConfigData
    {
        [field: SerializeField]
        public int LengthInDays { get; private set; } = 20;

        [field: SerializeField]
        public int Volume { get; private set; } = 20;

        [field: SerializeField]
        public float PriceBoostModifier { get; private set; } = 0.05f;

        [field: SerializeField, Range(0, 100)]
        public float ReputationReward { get; private set; } = 10f;

        [field: SerializeField, Range(-100, 0)]
        public float ReputationPenalty { get; private set; } = -20f;

        [field: SerializeField, Range(-100, 0)]
        public float GrowthPenalty { get; private set; } = -15f;

        public IMissionResult GetReward()
        {
            return new UpgradeMissionReward(ReputationReward);
        }

        public IMissionResult GetPenalty()
        {
            return new UpgradeMissionPenalty(ReputationPenalty, GrowthPenalty);
        }
    }
}