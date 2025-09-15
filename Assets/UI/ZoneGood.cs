using Data;
using NaughtyAttributes;
using UI.InventoryUI;
using UnityEngine;

namespace UI
{
    public sealed class ZoneGood : MonoBehaviour
    {
        [SerializeField, Required]
        private GoodCell tier1Cell;

        [SerializeField, Required]
        private GoodCell tier2Cell;

        public void SetUp(Good tier1, Good tier2)
        {
            tier1Cell.SetGood(tier1);
            tier2Cell.SetGood(tier2);
        }
    }
}