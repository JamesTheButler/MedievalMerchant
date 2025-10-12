using System;
using Common.Types;
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
        private Sprite isProducedIcon;

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
            ConstructionCellData tier1Good1,
            ConstructionCellData tier2Good1,
            ConstructionCellData tier1Good2,
            ConstructionCellData tier2Good2,
            ConstructionCellData tier3Good)
        {
            Tier3Good = tier3Good.Good;
            _isAlreadyBuilt = tier3Good.IsProduced;

            SetCell(tier1Cell1, tier1Good1);
            SetCell(tier2Cell1, tier2Good1);
            SetCell(tier1Cell2, tier1Good2);
            SetCell(tier2Cell2, tier2Good2);
            SetCell(tier3Cell, tier3Good);

            Deselect();
        }

        private void SetCell(GoodCell cell, ConstructionCellData data)
        {
            cell.SetEnabled(true);
            cell.SetGood(data.Good);
            if (data.IsProduced)
            {
                cell.SetCornerIcon(isProducedIcon);
            }
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