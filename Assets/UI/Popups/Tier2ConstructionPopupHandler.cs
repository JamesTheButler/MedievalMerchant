using System;
using Features.Towns;
using UI.InventoryUI;
using UI.InventoryUI.TownInventory;
using UnityEngine;

namespace UI.Popups
{
    public sealed class Tier2ConstructionPopupHandler : MonoBehaviour
    {
        [SerializeField]
        private Tier2ConstructionPopup popup;

        private readonly Lazy<Selection> _selection = new(() => Selection.Instance);

        public void Show(ProductionCell cell)
        {
            popup.Show();
            popup.transform.position = cell.transform.position;

            var town = _selection.Value.SelectedTown;
            popup.Setup(town, cell.Index);
        }

        public void Hide()
        {
            popup.Hide();
        }
    }
}