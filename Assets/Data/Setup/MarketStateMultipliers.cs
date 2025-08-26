using System.Collections.Generic;
using UnityEngine;

namespace Data.Setup
{
    [CreateAssetMenu(fileName = "MarketStateMultipliers", menuName = "Data/MarketStateMultipliers")]
    public sealed class MarketStateMultipliers : ScriptableObject
    {
        [SerializeField]
        private float highDemandMultiplier = .25f;

        [SerializeField]
        private float demandMultiplier = .5f;

        [SerializeField]
        private float supplyMultiplier = 2f;

        [SerializeField]
        private float highSupplyMultiplier = 3f;

        public IReadOnlyDictionary<MarketState, float> Multipliers => _multipliers ??= GenerateDictionary();

        private Dictionary<MarketState, float> _multipliers;

        private Dictionary<MarketState, float> GenerateDictionary()
        {
            return new Dictionary<MarketState, float>
            {
                { MarketState.HighDemand, highDemandMultiplier },
                { MarketState.Demand, demandMultiplier },
                { MarketState.Supply, supplyMultiplier },
                { MarketState.HighSupply, highSupplyMultiplier },
            };
        }
    }
}