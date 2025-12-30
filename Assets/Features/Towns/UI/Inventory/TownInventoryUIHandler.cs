using System;
using Common.Infrastructure;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Features.Towns.UI.Inventory
{
    public sealed class TownInventoryUIHandler : MonoBehaviour
    {
        [FormerlySerializedAs("inventoryUi"),SerializeField, Required]
        private TownUI townUi;

        private Selection _selection;

        private void Start()
        {
            townUi.Hide();
            townUi.Initialize();

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

            townUi.Show();
            townUi.Bind(town);
        }

        private void DeselectTown()
        {
            townUi.Unbind();
            townUi.Hide();
        }
    }
}