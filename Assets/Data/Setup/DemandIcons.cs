using System.Collections.Generic;
using UnityEngine;

namespace Data.Setup
{
    [CreateAssetMenu(fileName = "DemandIcons", menuName = "Data/DemandIcons")]
    public sealed class DemandIcons : ScriptableObject
    {
        [SerializeField]
        private Sprite highDemandIcon;

        [SerializeField]
        private Sprite demandIcon;

        [SerializeField]
        private Sprite supplyIcon;

        [SerializeField]
        private Sprite highSupplyIcon;

        public IReadOnlyDictionary<SupplyDemand, Sprite> Icons => _icons ??= GenerateDictionary();

        private Dictionary<SupplyDemand, Sprite> _icons;

        private Dictionary<SupplyDemand, Sprite> GenerateDictionary()
        {
            return new Dictionary<SupplyDemand, Sprite>
            {
                { SupplyDemand.HighDemand, highDemandIcon },
                { SupplyDemand.Demand, demandIcon },
                { SupplyDemand.Supply, supplyIcon },
                { SupplyDemand.HighSupply, highSupplyIcon },
            };
        }
    }
}