using System;
using UnityEngine;

namespace Features.Towns.Reputation.Config
{
    [Serializable]
    public sealed record ReputationRewardData
    {
        [field: SerializeField]
        public int TradeVolumePerReputationPoint { get; private set; }

        [field: SerializeField, Range(0, 100f)]
        public float Tier1ProductionBuilding { get; private set; }

        [field: SerializeField, Range(0, 100f)]
        public float Tier2ProductionBuilding { get; private set; }

        [field: SerializeField, Range(0, 100f)]
        public float Tier3ProductionBuilding { get; private set; }

        [field: SerializeField, Range(0, 100f)]
        public float TownUpgradeTier2 { get; private set; }

        [field: SerializeField, Range(0, 100f)]
        public float TownUpgradeTier3 { get; private set; }
        
        [field: SerializeField, Range(0, 100f)]
        public float MissionCompleted { get; private set; }

        [field: SerializeField, Range(100f, 0f)]
        public float MissionExpired { get; private set; }
    }
}