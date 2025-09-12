using Data;
using UI.InventoryUI;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Popups
{
    public sealed class BuildTier1PopupGroup : MonoBehaviour
    {
        [SerializeField]
        private InventoryCell tier1GoodIcon;

        [SerializeField]
        private InventoryCell tier2GoodIcon;
        [SerializeField]
        private Image selectionImage;

        public void Setup(Good tier1Good, Good tier2Good, bool isAlreadyBuilt)
        {
            tier1GoodIcon.SetGood(tier1Good);
            tier2GoodIcon.SetGood(tier2Good);
            // TODO: use isAlreadyBuilt
        }

        public void Select()
        {
            selectionImage.enabled = true;
        }
        
        public void Deselect()
        {
            selectionImage.enabled = false;
        }
    }
}