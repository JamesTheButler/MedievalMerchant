using System;
using UnityEngine;

namespace Features.Towns.Reputation.Data
{
    [Serializable]
    public sealed record ReputationRewardData
    {
        [field: SerializeField, Range(0, 100f)]
        public float Tier1ProductionBuilding { get; private set; }

        [field: SerializeField, Range(0, 100f)]
        public float Tier2ProductionBuilding { get; private set; }

        [field: SerializeField, Range(0, 100f)]
        public float Tier3ProductionBuilding { get; private set; }
    }
}