using System;
using Data.Towns;
using UI.InventoryUI;
using UnityEngine;

namespace UI.Popups
{
    public sealed class Tier1ConstructionPopupHandler : MonoBehaviour
    {
        [SerializeField]
        private Tier1ConstructionPopup popup;

        private readonly Lazy<Selection> _selection = new(() => Selection.Instance);

        public void Show(ProductionCell cell)
        {
            popup.Show();
            popup.transform.position = cell.transform.position;

            var town = _selection.Value.SelectedTown;
            popup.Setup(town, 1000);
        }

        public void Hide()
        {
            popup.Hide();
        }
    }
}