using System.Collections.Generic;
using UnityEngine;

namespace Data.Setup
{
    [CreateAssetMenu(fileName = "DemandMultipliers", menuName = "Data/DemandMultipliers")]
    public sealed class DemandMultipliers : ScriptableObject
    {
        [SerializeField] private float veryLowDemandMultiplier = .25f;
        [SerializeField] private float lowDemandMultiplier = .5f;
        [SerializeField] private float normalDemandMultiplier = 1.0f;
        [SerializeField] private float highDemandMultiplier = 1.5f;
        [SerializeField] private float veryHighDemandMultiplier = 3.0f;

        public IReadOnlyDictionary<Demand, float> Multipliers => _multipliers ??= GenerateMultiplierDictionary();

        private Dictionary<Demand, float> _multipliers;

        private Dictionary<Demand, float> GenerateMultiplierDictionary()
        {
            return new Dictionary<Demand, float>
            {
                { Demand.VeryLow, veryLowDemandMultiplier },
                { Demand.Low, lowDemandMultiplier },
                { Demand.Normal, normalDemandMultiplier },
                { Demand.High, highDemandMultiplier },
                { Demand.VeryHigh, veryHighDemandMultiplier },
            };
        }
    }
}