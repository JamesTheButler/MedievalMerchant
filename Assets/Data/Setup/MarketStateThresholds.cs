using System.Collections.Generic;
using UnityEngine;

namespace Data.Setup
{
    [CreateAssetMenu(fileName = "MarketStateThresholds", menuName = "Data/MarketStateThresholds")]
    public sealed class MarketStateThresholds : ScriptableObject
    {
        [SerializeField]
        private int highDemandThreshold = 10;

        [SerializeField]
        private int demandThreshold = 25;

        [SerializeField]
        private int supplyThreshold = 50;

        [SerializeField]
        private int highSupplyThreshold = 100;

        public IReadOnlyDictionary<MarketState, int> Thresholds => _thresholds ??= GenerateDictionary();

        private Dictionary<MarketState, int> _thresholds;

        private Dictionary<MarketState, int> GenerateDictionary()
        {
            return new Dictionary<MarketState, int>
            {
                { MarketState.HighDemand, highDemandThreshold },
                { MarketState.Demand, demandThreshold },
                { MarketState.Supply, supplyThreshold },
                { MarketState.HighSupply, highSupplyThreshold },
            };
        }
    }
}