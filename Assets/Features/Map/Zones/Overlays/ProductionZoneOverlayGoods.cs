using Common.Types;
using Common.UI.Elements;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Map.Zones.Overlays
{
    public sealed class ProductionZoneOverlayGoods : MonoBehaviour
    {
        [SerializeField, Required]
        private GoodCell tier1Cell, tier2Cell;

        public void SetUp(Good tier1, Good tier2)
        {
            tier1Cell.SetGood(tier1);
            tier2Cell.SetGood(tier2);
        }
    }
}