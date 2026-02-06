using System;
using Common.Types;
using Common.UI.Elements;
using Common.UI.Elements.Cells;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.Towns.Production.UI.Construction
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

        public void Setup(Good tier1Good, Good tier2Good)
        {
            Tier1Good = tier1Good;
            tier1GoodIcon.SetGood(tier1Good);
            tier2GoodIcon.SetGood(tier2Good);
            isBuiltImage.enabled = false;

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