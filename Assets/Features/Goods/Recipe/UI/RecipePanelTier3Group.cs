using Common.Types;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Goods.Recipe.UI
{
    public sealed class RecipePanelTier3Group : MonoBehaviour
    {
        [SerializeField, Required]
        private RecipePanelItem tier1ItemA, tier2ItemA, tier1ItemB, tier2ItemB, tier3Item;

        public void Setup(Good tier1GoodA, Good tier2GoodA, Good tier1GoodB, Good tier2GoodB, Good tier3Good)
        {
            tier1ItemA.Setup(tier1GoodA);
            tier2ItemA.Setup(tier2GoodA);
            tier1ItemB.Setup(tier1GoodB);
            tier2ItemB.Setup(tier2GoodB);
            tier3Item.Setup(tier3Good);
        }
    }
}