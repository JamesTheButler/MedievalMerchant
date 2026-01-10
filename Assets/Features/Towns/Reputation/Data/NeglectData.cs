using System;
using UnityEngine;

namespace Features.Towns.Reputation.Data
{
    [Serializable]
    public sealed record NeglectData
    {
        [field: SerializeField, Range(-100f, 0f)]
        public float ReputationCost { get; private set; }

        [field: SerializeField] 
        public int ActivationDelayInDays { get; private set; } = 90;
        
        [field: SerializeField]
        public int IntervalInDays { get; private set; } = 7;
        
        [field: SerializeField]
        public int ReputationThreshold { get; private set; } = 75;
    }
}