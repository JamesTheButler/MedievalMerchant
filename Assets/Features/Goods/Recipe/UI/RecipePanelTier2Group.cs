using Common.Types;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Goods.Recipe.UI
{
    public sealed class RecipePanelTier2Group : MonoBehaviour
    {
        [SerializeField, Required]
        private RecipePanelItem tier1Item, tier2Item;

        public void Setup(Good tier1Good, Good tier2Good)
        {
            tier1Item.Setup(tier1Good);
            tier2Item.Setup(tier2Good);
        }
    }
}