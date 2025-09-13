using TMPro;
using UnityEngine;

namespace UI.InventoryUI
{
    public class InventoryCellBase : GoodCellBase
    {
        [SerializeField]
        private TMP_Text amountText;

        public void SetAmount(int amount)
        {
            if (amount <= 0)
            {
                amountText?.gameObject.SetActive(false);
            }
            else
            {
                amountText?.gameObject.SetActive(true);
                amountText?.SetText(amount.ToString());
            }

            SetEnabled(amount > 0);
        }
    }
}