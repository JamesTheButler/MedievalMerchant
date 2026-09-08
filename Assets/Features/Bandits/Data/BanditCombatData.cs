using System;
using Common.Config.Sampling;
using UnityEngine;

namespace Features.Bandits.Data
{
    [Serializable]
    public sealed class BanditCombatData
    {
        [field: SerializeReference, SubclassSelector]
        public ISampler HitFactorSampler { get; private set; }

        [field: SerializeField]
        public float EngagementRadius { get; private set; } = 1f;

        [field: SerializeField]
        public int PlayerRecoveringDurationDays { get; private set; } = 5;
    }
}