using System;
using UnityEngine;

namespace Features.Bandits.Data
{
    [Serializable]
    public sealed class BanditRushData
    {
        [field: SerializeField]
        public float DetectionRadius { get; private set; } = 2.5f;
    }
}
