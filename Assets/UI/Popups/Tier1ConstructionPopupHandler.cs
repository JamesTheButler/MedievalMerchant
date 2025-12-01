using System;
using Features.Towns;
using Infrastructure;
using UI.InventoryUI.TownInventory;
using UnityEngine;

namespace UI.Popups
{
    public sealed class Tier1ConstructionPopupHandler : MonoBehaviour
    {
        [SerializeField]
        private Tier1ConstructionPopup popup;

        private readonly Lazy<Selection> _selection = new(() => GameplayContext.Selection);

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