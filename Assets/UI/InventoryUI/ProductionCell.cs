using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.InventoryUI
{
    public sealed class ProductionCell : InventoryCellBase
    {
        private bool _isLocked = true;

        [SerializeField]
        private UnityEvent unlockButtonClicked;

        [SerializeField]
        private Button unlockButton;

        private void Start()
        {
            tooltipHandler.SetTooltip("Build production building.");
            tooltipHandler.SetEnabled(true);
        }

        public void InvokeUnlockButtonClicked()
        {
            _isLocked = false;
            unlockButton.gameObject.SetActive(false);
            // TODO: this should open a popup to select which good to build
            unlockButtonClicked?.Invoke();
            tooltipHandler.SetEnabled(false);
        }
    }
}