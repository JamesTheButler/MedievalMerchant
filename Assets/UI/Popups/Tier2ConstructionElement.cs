using Data;
using NaughtyAttributes;
using UI.InventoryUI;
using UnityEngine;

namespace UI.Popups
{
    public sealed class Tier2ConstructionElement : MonoBehaviour
    {
        [SerializeField, Required]
        private GoodCell tier1GoodIcon;

        [SerializeField, Required]
        private GoodCell tier2GoodIcon;
        
        public void Setup(Good tier1Good, Good tier2Good)
        {
            tier1GoodIcon.SetGood(tier1Good);
            tier2GoodIcon.SetGood(tier2Good);
        }
    }
}