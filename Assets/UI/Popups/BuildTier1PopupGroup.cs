using System;
using Data;
using NaughtyAttributes;
using UI.InventoryUI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Popups
{
    public sealed class BuildTier1PopupGroup : MonoBehaviour, IPointerClickHandler
    {
        public event Action Clicked;

        [SerializeField, Required]
        private GoodCell tier1GoodIcon;

        [SerializeField, Required]
        private GoodCell tier2GoodIcon;

        [SerializeField, Required]
        private Image selectionImage;

        [SerializeField, Required]
        private Image isBuiltImage;

        public Good Tier1Good { get; private set; }
        
        public void Setup(Good tier1Good, Good tier2Good, bool isAlreadyBuilt)
        {
            Tier1Good = tier1Good;
            tier1GoodIcon.SetGood(tier1Good);
            tier2GoodIcon.SetGood(tier2Good);
            isBuiltImage.enabled = isAlreadyBuilt;
            // TODO: isAlreadyBuilt should make the group unclickable and add a tooltip saying (already built)
            Deselect(); // initially, it shouldn't be selected
        }

        public void Select()
        {
            selectionImage.enabled = true;
        }

        public void Deselect()
        {
            selectionImage.enabled = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Clicked?.Invoke();
        }
    }
}