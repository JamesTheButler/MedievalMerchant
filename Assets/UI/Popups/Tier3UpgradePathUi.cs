using Data;
using UI.InventoryUI;
using UnityEngine;

namespace UI.Popups
{
    public sealed class Tier3UpgradePathElement : MonoBehaviour
    {
        [SerializeField]
        private GoodCell tier1Cell1;

        [SerializeField]
        private GoodCell tier2Cell1;

        [SerializeField]
        private GoodCell tier1Cell2;

        [SerializeField]
        private GoodCell tier2Cell2;

        [SerializeField]
        private GoodCell tier3Cell;

        public void Setup(Good tier1Good1, Good tier2Good1, Good tier1Good2, Good tier2Good2, Good tier3Good1)
        {
            tier1Cell1.SetEnabled(true);
            tier1Cell1.SetGood(tier1Good1);

            tier2Cell1.SetEnabled(true);
            tier2Cell1.SetGood(tier2Good1);

            tier1Cell2.SetEnabled(true);
            tier1Cell2.SetGood(tier1Good2);

            tier2Cell2.SetEnabled(true);
            tier2Cell2.SetGood(tier2Good2);

            tier3Cell.SetEnabled(true);
            tier3Cell.SetGood(tier3Good1);
        }

        public void Reset()
        {
            tier1Cell1.SetGood(null);
            tier2Cell1.SetGood(null);
            tier1Cell2.SetGood(null);
            tier2Cell2.SetGood(null);
            tier3Cell.SetGood(null);
        }
    }
}