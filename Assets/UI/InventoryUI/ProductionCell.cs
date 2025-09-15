using Data;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.InventoryUI
{
    public sealed class ProductionCell : InventoryCellBase
    {
        [SerializeField]
        public UnityEvent unlockButtonClicked;

        [SerializeField]
        private Button unlockButton;

        private void Start()
        {
            Lock();
        }

        protected override void OnSetGood(Good? good)
        {
            base.OnSetGood(good);
            if (good != null)
            {
                Unlock();
            }
        }

        public void Unlock()
        {
            unlockButton.gameObject.SetActive(false);
            tooltipHandler.SetEnabled(Good != null);
        }

        public void Lock()
        {
            unlockButton.gameObject.SetActive(true);
            tooltipHandler.SetTooltip("Build production building.");
            tooltipHandler.SetEnabled(true);
        }

        public void InvokeUnlockButtonClicked()
        {
            unlockButtonClicked?.Invoke();
            Unlock();
        }
    }
}