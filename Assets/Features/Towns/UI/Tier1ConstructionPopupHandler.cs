using System;
using Infrastructure;
using UI.InventoryUI.TownInventory;
using UnityEngine;

namespace Features.Towns.UI
{
    public sealed class Tier1ConstructionPopupHandler : MonoBehaviour
    {
        [SerializeField]
        private Tier1ConstructionPopup popup;

        private readonly Lazy<Selection> _selection = new(() => GameplayContext.Instance.Selection);

        public void Show(ProductionCell cell)
        {
            popup.Show();
            popup.transform.position = cell.transform.position;

            var town = _selection.Value.SelectedTown;
            popup.Setup(town, cell.Index);

            _selection.Value.TownSelected += _ => Hide();
        }

        private void Hide()
        {
            popup.Hide();
        }
    }
}