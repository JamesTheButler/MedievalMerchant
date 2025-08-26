using System.Collections.Generic;
using UnityEngine;

namespace Data.Setup
{
    [CreateAssetMenu(fileName = "DemandThresholds", menuName = "Data/DemandThresholds")]
    public sealed class DemandThresholds : ScriptableObject
    {
        [SerializeField] private int veryLowDemandThreshold = 50;
        [SerializeField] private int lowDemandThreshold = 40;
        [SerializeField] private int normalDemandThreshold = 30;
        [SerializeField] private int highDemandThreshold = 20;
        [SerializeField] private int veryHighDemandThreshold = 10;

        public IReadOnlyDictionary<SupplyDemand, int> Thresholds => _thresholds ??= GenerateDictionary();

        private Dictionary<SupplyDemand, int> _thresholds;

        private Dictionary<SupplyDemand, int> GenerateDictionary()
        {
            return new Dictionary<SupplyDemand, int>
            {
                { SupplyDemand.VeryLow, veryLowDemandThreshold },
                { SupplyDemand.Low, lowDemandThreshold },
                { SupplyDemand.Normal, normalDemandThreshold },
                { SupplyDemand.High, highDemandThreshold },
                { SupplyDemand.VeryHigh, veryHighDemandThreshold },
            };
        }
    }
}