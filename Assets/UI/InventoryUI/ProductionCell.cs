using System;
using Data;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InventoryUI
{
    public sealed class ProductionCell : InventoryCellBase
    {
        public event Action UnlockButtonClicked;

        [SerializeField]
        private Button unlockButton;

        public int Index { get; set; }
        
        private void Awake()
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
            tooltipHandler.SetEnabled(false);
            UnlockButtonClicked?.Invoke();
        }
    }
}