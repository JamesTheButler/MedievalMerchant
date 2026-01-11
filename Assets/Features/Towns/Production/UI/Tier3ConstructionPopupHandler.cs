using System;
using Common.Infrastructure;
using Features.Towns.UI;
using UnityEngine;

namespace Features.Towns.Production.UI
{
    public sealed class Tier3ConstructionPopupHandler : MonoBehaviour
    {
        [SerializeField]
        private Tier3ConstructionPopup popup;

        private readonly Lazy<Selection> _selection = new(() => GameplayContext.Instance.Selection);

        private void Start()
        {
            _selection.Value.SelectedTown.Observe(OnSelectionChanged);
        }

        private void OnDestroy()
        {
            _selection.Value.SelectedTown.StopObserving(OnSelectionChanged);
        }

        public void Show(ProductionCell cell)
        {
            popup.Open();
            popup.transform.position = cell.transform.position;

            var town = _selection.Value.SelectedTown;
            popup.Setup(town, cell.Index);
        }

        private void OnSelectionChanged(Town _)
        {
            Hide();
        }

        private void Hide()
        {
            popup.Close();
        }
    }
}