using System;
using UnityEngine;

namespace Features.Bandits.Data
{
    [Serializable]
    public sealed class BanditRestData
    {
        [field: SerializeField]
        public int DaysPerRestCycle { get; private set; } = 5;

        [field: SerializeField]
        public int RelocationDistanceMin { get; private set; } = 2;

        [field: SerializeField]
        public int RelocationDistanceMax { get; private set; } = 5;

        [field: SerializeField]
        public int RelocationCooldownCycles { get; private set; } = 3;

        [field: SerializeField, Range(0f, 1f)]
        public float RelocationChancePerCycle { get; private set; } = 0.15f;
    }
}
