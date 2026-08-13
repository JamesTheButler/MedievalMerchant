using System;
using UnityEngine;

namespace Features.Bandits.Data
{
    /// <summary>
    /// Combat values that apply globally, regardless of bandit tier or behavior state.
    /// </summary>
    [Serializable]
    public sealed class BanditCombatData
    {
        [field: SerializeField]
        public float HitFactorMin { get; private set; } = 0.5f;

        [field: SerializeField]
        public float HitFactorMax { get; private set; } = 1.5f;

        [field: SerializeField]
        public float EngagementRadius { get; private set; } = 1f;

        [field: SerializeField]
        public int PlayerRecoveringDurationDays { get; private set; } = 5;
    }
}
