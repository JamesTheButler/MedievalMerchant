using System;
using Data;
using NaughtyAttributes;
using UI.InventoryUI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Popups
{
    public sealed class Tier3UpgradePathElement : MonoBehaviour, IPointerClickHandler
    {
        public event Action Clicked;

        [SerializeField, Required]
        private GoodCell tier1Cell1;

        [SerializeField, Required]
        private GoodCell tier2Cell1;

        [SerializeField, Required]
        private GoodCell tier1Cell2;

        [SerializeField, Required]
        private GoodCell tier2Cell2;

        [SerializeField, Required]
        private GoodCell tier3Cell;

        [SerializeField, Required]
        private Image selectionImage;

        public Good Tier3Good { get; private set; }

        private bool _isAlreadyBuilt;

        public void Setup(
            Good tier1Good1,
            Good tier2Good1,
            Good tier1Good2,
            Good tier2Good2,
            Good tier3Good,
            bool isAlreadyBuilt)
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
            tier3Cell.SetGood(tier3Good);
            Tier3Good = tier3Good;

            _isAlreadyBuilt = isAlreadyBuilt;

            Deselect();
        }

        public void Reset()
        {
            tier1Cell1.SetGood(null);
            tier2Cell1.SetGood(null);
            tier1Cell2.SetGood(null);
            tier2Cell2.SetGood(null);
            tier3Cell.SetGood(null);
        }

        public void Select()
        {
            if (_isAlreadyBuilt)
                return;

            selectionImage.enabled = true;
        }

        public void Deselect()
        {
            selectionImage.enabled = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_isAlreadyBuilt)
                return;

            Clicked?.Invoke();
        }
    }
}