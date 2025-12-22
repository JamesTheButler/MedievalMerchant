using System;
using Common.Infrastructure;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Towns.UI.Inventory
{
    public sealed class TownInventoryUIHandler : MonoBehaviour
    {
        [SerializeField, Required]
        private TownInventoryUI inventoryUi;

        private Selection _selection;

        private void Start()
        {
            inventoryUi.Hide();
            inventoryUi.Initialize();

            _selection = GameplayContext.Instance.Selection;
            _selection.SelectedTown.Observe(SelectTown, false);
        }

        private void OnDestroy()
        {
            _selection.SelectedTown.StopObserving(SelectTown);
        }

        private void SelectTown(Town town)
        {
            if (town == null)
            {
                DeselectTown();
                return;
            }

            inventoryUi.Show();
            inventoryUi.Bind(town);
        }

        private void DeselectTown()
        {
            inventoryUi.Unbind();
            inventoryUi.Hide();
        }
    }
}