using System.Collections.Generic;
using UnityEngine;

namespace Data.Setup
{
    [CreateAssetMenu(fileName = "MarketStateIcons", menuName = "Data/MarketStateIcons")]
    public sealed class MarketStateIcons : ScriptableObject
    {
        [SerializeField]
        private Sprite highDemandIcon;

        [SerializeField]
        private Sprite demandIcon;

        [SerializeField]
        private Sprite supplyIcon;

        [SerializeField]
        private Sprite highSupplyIcon;

        public IReadOnlyDictionary<MarketState, Sprite> Icons => _icons ??= GenerateDictionary();

        private Dictionary<MarketState, Sprite> _icons;

        private Dictionary<MarketState, Sprite> GenerateDictionary()
        {
            return new Dictionary<MarketState, Sprite>
            {
                { MarketState.HighDemand, highDemandIcon },
                { MarketState.Demand, demandIcon },
                { MarketState.Supply, supplyIcon },
                { MarketState.HighSupply, highSupplyIcon },
            };
        }
    }
}