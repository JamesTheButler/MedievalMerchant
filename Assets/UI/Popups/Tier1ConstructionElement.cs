using System;
using Data;
using NaughtyAttributes;
using UI.InventoryUI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Popups
{
    public sealed class Tier1ConstructionElement : MonoBehaviour, IPointerClickHandler
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

        private bool _isAlreadyBuilt;

        public void Setup(Good tier1Good, Good tier2Good, bool isAlreadyBuilt)
        {
            Tier1Good = tier1Good;
            _isAlreadyBuilt = isAlreadyBuilt;

            tier1GoodIcon.SetGood(tier1Good);
            tier2GoodIcon.SetGood(tier2Good);
            isBuiltImage.enabled = _isAlreadyBuilt;

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
            if (_isAlreadyBuilt) return;
            Clicked?.Invoke();
        }
    }
}