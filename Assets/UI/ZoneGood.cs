using Data;
using NaughtyAttributes;
using UnityEngine;

namespace UI
{
    public sealed class ZoneGood : MonoBehaviour
    {
        [SerializeField, Required]
        private InventoryCell tier1Cell;

        [SerializeField, Required]
        private InventoryCell tier2Cell;

        private void Start()
        {
            tier1Cell.SetIsProduced(false);
            tier1Cell.ShowText(false);
            tier1Cell.SetAmount(1);

            tier2Cell.SetIsProduced(false);
            tier2Cell.ShowText(false);
            tier2Cell.SetAmount(1);
        }

        public void SetTier1Good(Good good)
        {
            tier1Cell.SetGood(good);
        }

        public void SetTier2Good(Good good)
        {
            tier2Cell.SetGood(good);
        }
    }
}